using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using LMS.Common.Infos;
using Syncfusion.XlsIO;

namespace Compilers
{
    public class ExcelCompiler : CloudCompiler
    {
        internal ExcelCompiler(string folder) : base(folder) { }
        public override ExecutionResult Run(RunInfo runInfo, CompilerInfo compilerInfo)
        {
            ExecutionResult er = new ExecutionResult
            {
                Grade = 0,
                Compiled = true
            };

            if (!string.IsNullOrEmpty(runInfo.Code) && !string.IsNullOrEmpty(runInfo.ExpectedResult))
            {
                List<string> differences = new List<string>();

                ExcelEngine excelEngine = new ExcelEngine();
                IWorkbook studWorkbook = excelEngine.Excel.Workbooks.Open(GetStreamFromBinary(runInfo.Code));
                IWorksheets studSheets = studWorkbook.Worksheets;

                IWorkbook solWorkbook = excelEngine.Excel.Workbooks.Open(GetStreamFromBinary(runInfo.ExpectedResult));
                IWorksheets solSheets = solWorkbook.Worksheets;


                if (studSheets.Count != solSheets.Count)
                {
                    differences.Add("Expected number of Worksheet: " + solSheets.Count + ", Actual number of Worksheet: " + studSheets.Count);
                }

                for (int i = 0; i < solSheets.Count; i++)
                {
                    IWorksheet sheet = solSheets[i];

                    // Get worksheets by name
                    IWorksheet solutionWorkSheet = solSheets.FirstOrDefault(x => x.Name == sheet.Name);
                    IWorksheet studentWorksheet = studSheets.FirstOrDefault(x => x.Name == sheet.Name);

                    if (studentWorksheet == null)
                    {
                        differences.Add("No Worksheet named " + sheet.Name + " was found in the file.");
                        continue;
                    }


                    //---------------compare charts-------------------------------------

                    IChartShapes solutionCharts = solutionWorkSheet.Charts;
                    IChartShapes studentCharts = studentWorksheet.Charts;

                    if (solutionCharts.Count != studentCharts.Count)
                    {
                        differences.Add("Your file must have " + solutionCharts.Count + " chart(s)");
                    }
                    else if (solutionCharts.Count != 0)
                    {
                        for (int j = 0; j < solutionCharts.Count; j++)
                        {
                            if (solutionCharts[j].ChartType != studentCharts[j].ChartType)
                            {
                                differences.Add("In " + sheet.Name + ", chart #" + (j + 1) + " has wrong type");
                            }
                            else
                            {
                                List<IChartSerie> solutionChartData = solutionCharts[j].Series.ToList();
                                List<IChartSerie> studentChartData = studentCharts[j].Series.ToList();

                                bool chartDataIsEqual = true;
                                if (solutionChartData.Count != studentChartData.Count)
                                {
                                    chartDataIsEqual = false;
                                }
                                else
                                {
                                    for (int k = 0; k < solutionChartData.Count; k++)
                                    {

                                        IRange[] solutionSeries = solutionChartData[k].Values.Cells;
                                        IRange[] studentSeries = studentChartData[k].Values.Cells;
                                        if (solutionSeries.Count() != studentSeries.Count())
                                        {
                                            chartDataIsEqual = false;
                                            break;
                                        }
                                        else
                                        {
                                            //----------check trendlines------
                                            IChartTrendLines solutionTrendlines = solutionChartData[k].TrendLines;
                                            IChartTrendLines studentTrendlines = studentChartData[k].TrendLines;
                                            if (solutionTrendlines.Count != studentTrendlines.Count)
                                            {
                                                differences.Add("In " + sheet.Name + ", chart #" + (j + 1) + " must have " + solutionTrendlines.Count + " trendline(s)");
                                            }
                                            else
                                            {
                                                for (int n = 0; n < solutionTrendlines.Count; n++)
                                                {
                                                    if (solutionTrendlines[n].Type != studentTrendlines[n].Type)
                                                    {
                                                        differences.Add("In " + sheet.Name + ", chart #" + (j + 1) + " , trendline # " + (n + 1) + " has a wrong type");
                                                    }
                                                }

                                            }

                                            //----------check chart data------
                                            for (int m = 0; m < solutionSeries.Count(); m++)
                                            {
                                                if (solutionSeries[m].Value != studentSeries[m].Value)
                                                {
                                                    chartDataIsEqual = false;
                                                    break;
                                                }
                                            }
                                            if (!chartDataIsEqual)
                                            {
                                                break;
                                            }
                                        }
                                    }
                                    if (!chartDataIsEqual)
                                    {
                                        differences.Add("In " + sheet.Name + ", chart #" + (j + 1) + " has wrong data");
                                    }
                                }
                            }
                        }
                    }

                    IRange[] solutionData = solutionWorkSheet.Cells;
                    IRange[] studentData = studentWorksheet.Cells;
                    int solutionCount = solutionData.Count();
                    int studentCount = studentData.Count();

                    Dictionary<string, string> solutionValues = new Dictionary<string, string>();

                    foreach (IRange cell in solutionData)
                    {
                        if (cell.Value != "")
                        {
                            solutionValues.Add(CellName(cell.AddressR1C1Local), ReplaceFuntion(cell.Value));
                        }
                    }

                    foreach (IRange cell in studentData)
                    {
                        if (cell.Value == "")
                        {
                            continue;
                        }

                        string name = CellName(cell.AddressR1C1Local);
                        if (solutionValues.ContainsKey(name))
                        {
                            if (cell.Value != solutionValues[name])
                            {
                                differences.Add("Sheet: " + sheet.Name + "; Cell: " + name + "; Value: " + cell.Value + " is different. Expected: " + solutionValues[name]);
                            }
                            _ = solutionValues.Remove(name);
                        }
                        else
                        {
                            differences.Add("Sheet: " + sheet.Name + "; Cell: " + name + "; Value: " + cell.Value + " is different. Expected: Empty Cell");
                        }
                    }
                    foreach (KeyValuePair<string, string> row in solutionValues)
                    {
                        differences.Add("Sheet: " + sheet.Name + "; Cell: " + row.Key + "; Missing Value: " + row.Value);
                    }
                    er.Grade += 100 - (100 * differences.Count / Math.Max(solutionCount, studentCount));
                }
                er.Grade /= solSheets.Count;

                er.Succeeded = true;
                er.ActualErrors = differences;
                // Close document
                studWorkbook.Close();
                solWorkbook.Close();
            }

            return er;
        }

        private MemoryStream GetStreamFromBinary(string binaryStr)
        {
            byte[] byteFile = Convert.FromBase64String(binaryStr);
            MemoryStream ms = new MemoryStream();
            ms.Write(byteFile, 0, byteFile.Length);
            ms.Position = 0;
            return ms;
        }
        // Get column header's field name
        private static string CellName(string addressR1C1)
        {
            string name;
            string row;
            int col;
            int rPosition = addressR1C1.IndexOf("R");
            int cPosition = addressR1C1.IndexOf("C");
            try
            {
                row = addressR1C1.Substring(rPosition + 1, cPosition - rPosition - 1);
                col = int.Parse(addressR1C1.Substring(cPosition + 1)) - 1;
            }
            catch (Exception /*e*/)
            {
                return addressR1C1;
            }

            int dec = col / 26;
            int num = col % 26;

            name = char.ConvertFromUtf32(Convert.ToInt32('A') + num);

            if (dec > 0)
            {
                name = char.ConvertFromUtf32(Convert.ToInt32('A') + dec) + name;
            }

            name += row;
            return name;
        }
    }
}


