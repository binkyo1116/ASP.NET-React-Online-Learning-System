using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Compilers
{
    public static class ProcessHelper
    {
        private static readonly ISet<string> Ignore = new HashSet<string>()
        {
            "WARNING: An illegal reflective access operation has occurred",
            "WARNING: Illegal reflective access by org.apache.spark.unsafe.Platform(file:/C:/Spark/jars/spark-unsafe_2.12-3.2.1.jar) to constructor java.nio.DirectByteBuffer(long, int)",
            "WARNING: Please consider reporting this to the maintainers of org.apache.spark.unsafe.Platform",
            "WARNING: Use --illegal-access=warn to enable warnings of further illegal reflective access operations",
            "WARNING: All illegal access operations will be denied in a future release",
            "Using Spark's default log4j profile: org/apache/spark/log4j-defaults.properties",
            "Setting default log level to \"WARN\".",
            "To adjust logging level use sc.setLogLevel(newLevel). For SparkR, use setLogLevel(newLevel).",
            "WARNING: Illegal reflective access by org.apache.spark.unsafe.Platform (file:/C:/Spark/jars/spark-unsafe_2.12-3.2.1.jar) to constructor java.nio.DirectByteBuffer(long,int)",
            "WARN NativeCodeLoader: Unable to load native-hadoop library for your platform... using builtin-java classes where applicable",
            "ProcfsMetricsGetter: Exception when trying to compute pagesize, as a result reporting of ProcessTree metrics is stopped",
            "The system cannot execute the specified program.",
            "WARN Utils: Service 'SparkUI' could not bind on port 4040. Attempting port 4041.",
            "Stage ",
            "WARN package: Truncated the string representation of a plan since it was too large. This behavior can be adjusted by setting 'spark.sql.debug.maxToStringFields'.",
            "Access is denied.",
            "WARN Utils: Service 'SparkUI' could not bind on port",
            "WARN InstanceBuilder$NativeBLAS",
            "WARN InstanceBuilder$NativeLAPACK",
            "WARN Instrumentation:"
        };

        private static StringBuilder sbErrors;
        private static StringBuilder sbOutput;
        public static bool RunProcess(string workingDirectory, string fileName, string arguments, int timeOut, out string output, out string errors)
        {
            ProcessStartInfo executionStartInfo = new ProcessStartInfo()
            {
                WorkingDirectory = workingDirectory,
                Arguments = arguments,
                FileName = fileName,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false
            };

            Process process = new Process
            {
                StartInfo = executionStartInfo,
                EnableRaisingEvents = true
            };
            process.OutputDataReceived += OutputDataHander;
            process.ErrorDataReceived += ErrorDataHandler;

            try
            {

                _ = process.Start();
            }
            catch (Exception ex)
            {
                errors = ex.ToString();
                output = "";
                return false;
            }
            sbErrors = new StringBuilder();
            sbOutput = new StringBuilder();
            process.BeginErrorReadLine();
            process.BeginOutputReadLine();

            bool success = process.WaitForExit(timeOut * 1000);

            if (!success)
            {
                _ = sbErrors.AppendLine("Time out 1");
                process.Kill();
            }
            if (!process.HasExited)
            {
                _ = sbErrors.AppendLine("Time out 2");
            }
            errors = sbErrors.ToString();
            output = sbOutput.ToString();
            return success;
        }
        private static void ErrorDataHandler(object sender, DataReceivedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(e.Data))
            {
                bool ignore = false;
                foreach (string i in Ignore)
                {
                    if (e.Data.Contains(i))
                    {
                        ignore = true;
                    }
                }
                if (!ignore)
                {
                    _ = sbErrors.AppendLine("Error: " + e.Data);
                }
            }
        }

        private static void OutputDataHander(object sender, DataReceivedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(e.Data))
            {
                bool ignore = false;
                foreach (string i in Ignore)
                {
                    if (e.Data.Contains(i))
                    {
                        ignore = true;
                    }
                }
                if (!ignore)
                {
                    _ = sbOutput.AppendLine(e.Data);
                }
            }
        }
    }
}
