using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using CompilerFunctionsTemp;
using LMSLibrary;
using LMS.Common.ViewModels;
using LMS.Common.HelperModels;
using LMS.Common.Infos;
using LMS.Common.SharedFunctions;

namespace CompilerFunctions
{
    public static partial class Compiler
    {
        private static VmStudent student;
        private static VmCodingProblem codingProblem;
        private static string codingProblemInstanceInstructions;
        private static string codingProblemInstanceSolution;
        private static string codingProblemInstanceTestCode;
        private static string codingProblemInstanceExpectedOutput;
        private static string codingProblemInstanceBefore;
        private static string codingProblemInstanceAfter;
        private static string codingProblemInstanceScript;
        private static string codingProblemInstanceTestCodeForStudent;
        private static HashSet<(string, string, string)> instancesData;

        private static void Load(string hash, int codingProblemId)
        {
            student = GetStudentInfoByHash(hash);
            codingProblem = GetCodingProblem(codingProblemId);
            List<VmVariableValue> vmVariableValues = GetVmVariableValue(codingProblem.Id);
            if (vmVariableValues.Count() == 0)
            {
                string studentId = GetStudentId();
                codingProblemInstanceInstructions = SharedFunctions.InitializeVariablesInString(codingProblem.Instructions, instancesData, studentId);
                codingProblemInstanceSolution = SharedFunctions.InitializeVariablesInString(codingProblem.Solution, instancesData, studentId);
                codingProblemInstanceTestCode = SharedFunctions.InitializeVariablesInString(codingProblem.TestCode, instancesData, studentId);
                codingProblemInstanceExpectedOutput = SharedFunctions.InitializeVariablesInString(codingProblem.ExpectedOutput, instancesData, studentId);
                codingProblemInstanceBefore = SharedFunctions.InitializeVariablesInString(codingProblem.Before, instancesData, studentId);
                codingProblemInstanceAfter = SharedFunctions.InitializeVariablesInString(codingProblem.After, instancesData, studentId);
                codingProblemInstanceScript = SharedFunctions.InitializeVariablesInString(codingProblem.Script, instancesData, studentId);
                codingProblemInstanceTestCodeForStudent = codingProblem.TestCodeForStudent != null ? SharedFunctions.InitializeVariablesInString(codingProblem.TestCodeForStudent, instancesData, studentId) : "";
            }
            else
            {
                HashSet<VmCodingProblemInstance> matchingInstances = GetCodingProblemInstance(codingProblem.Id, student.StudentId).ToHashSet();
                HashSet<VmCodingProblemInstance> matchingInstancesWithoutOccurrences = matchingInstances.Where(x => x.occurrenceNumber == null).ToHashSet();
                HashSet<VmVariableValue> missingVars = new HashSet<VmVariableValue>();
                
                foreach (var v in vmVariableValues)
                {
                    if (!matchingInstancesWithoutOccurrences.Where(ins => ins.idVariableValue == v.idVariableValue).Any())
                    {
                        missingVars.Add(v);
                    }
                }

                var noMemoryInstances = GetInstancesWithoutMemory(codingProblem);//problem
                var singleInstanceWithoutMemory = noMemoryInstances.Where(x => x.occurenceNumber == null).Select(x => x.varName).ToList();
                HashSet<VmCodingProblemInstance> newInstances = CreateCodingProblemInstance(missingVars, singleInstanceWithoutMemory);

                matchingInstances.UnionWith(newInstances);

                var multipleInstances = GetMultipleInstances(codingProblem);//problem
                //TODO delete variables that are no longer in the coding problem
                HashSet<VmCodingProblemInstance> matchingInstancesWithOccurrences = matchingInstances.Where(x => x.occurrenceNumber != null).ToHashSet();
                var missingInstances = new HashSet<(string varName, string possibleValues, int numberOfInstances)>();
                foreach (var (varName, instances) in multipleInstances)
                {
                    int numberOfInstances = 0;
                    foreach (var instance in instances)
                    {
                        if (!matchingInstancesWithOccurrences.Where(ins => ins.VarName.Equals(varName) && ins.occurrenceNumber == int.Parse(instance)).Any())
                        {
                            numberOfInstances++;
                        }
                    }
                    //string possibleValues = codingProblem.VariableValues.First(x => x.VarName.Equals(varName)).possibleValues;
                    string possibleValues = GetVariableValuesId(codingProblem.Id, varName).possibleValues;
                    if (numberOfInstances > 0)
                    {
                        missingInstances.Add((varName, possibleValues, numberOfInstances));
                    }
                }

                var multipleInstanceWithoutMemory = noMemoryInstances.Where(x => x.occurenceNumber != null).ToHashSet();
                if (missingInstances.Count > 0)
                {
                    HashSet<VmCodingProblemInstance> newMultiInstances = CreateCodingProblemMultiInstance(missingInstances, multipleInstances, multipleInstanceWithoutMemory);
                    matchingInstances.UnionWith(newMultiInstances);
                }

                instancesData = new HashSet<(string varName, string varValue, string ocurrenceNumber)>();
                foreach (var v in matchingInstances)
                {
                    var occurrenceNumber = v.occurrenceNumber == null ? null : v.occurrenceNumber.ToString();
                    instancesData.Add((v.VarName, v.VarValue, occurrenceNumber));
                }

                var tuples = GetInstancesTuples(codingProblem, instancesData);//Problem
                instancesData.UnionWith(tuples);
                string studentId = GetStudentId();
                codingProblemInstanceInstructions = SharedFunctions.InitializeVariablesInString(codingProblem.Instructions, instancesData, studentId);
                codingProblemInstanceSolution = SharedFunctions.InitializeVariablesInString(codingProblem.Solution, instancesData, studentId);
                codingProblemInstanceTestCode = SharedFunctions.InitializeVariablesInString(codingProblem.TestCode, instancesData, studentId);
                codingProblemInstanceExpectedOutput = SharedFunctions.InitializeVariablesInString(codingProblem.ExpectedOutput, instancesData, studentId);
                codingProblemInstanceBefore = SharedFunctions.InitializeVariablesInString(codingProblem.Before, instancesData, studentId);
                codingProblemInstanceAfter = SharedFunctions.InitializeVariablesInString(codingProblem.After, instancesData, studentId);
                codingProblemInstanceScript = SharedFunctions.InitializeVariablesInString(codingProblem.Script, instancesData, studentId);
                codingProblemInstanceTestCodeForStudent = codingProblem.TestCodeForStudent != null ? SharedFunctions.InitializeVariablesInString(codingProblem.TestCodeForStudent, instancesData, studentId) : "";
            }
        }
        //TODO: Ask Marcelo what is expected to substitute studentid constant
        private static string GetStudentId()
        {
            string studentId = student.Email;

            if (studentId.Contains('@'))
            {
                studentId = student.Email.Split('@')[0];
            }

            studentId = studentId.Replace("-", "");
            studentId = studentId.Replace(".", "");
            return studentId;
        }

        public static VmStudent GetStudentInfoByHash(string hashedPassword)
        {
            string sqlQueryStudent = $@"select StudentId, Name, Email,Test
                                        from Student
                                        where Hash = '{hashedPassword}'"; // AND (Password = '{hashedPassword}' OR Password = '{password}');

            var studentData = SQLHelper.RunSqlQuery(sqlQueryStudent);
            VmStudent studentinfo = null;

            if (studentData.Count > 0)
            {
                List<object> st = studentData[0];

                studentinfo = new VmStudent
                {
                    StudentId = (int)st[0],
                    Name = st[1].ToString(),
                    Email = st[2].ToString(),
                    Test = (st[3] != System.DBNull.Value && (bool)st[3])
                };

            }


            return studentinfo;
        }

        public static VmCodingProblem GetCodingProblem(int codingProblemId)
        {
            string sqlQueryCoding = $@"select * from CodingProblem where Id = {codingProblemId}"; // AND (Password = '{hashedPassword}' OR Password = '{password}');

            var codingData = SQLHelper.RunSqlQuery(sqlQueryCoding);
            VmCodingProblem codingProblem = null;

            if (codingData.Count > 0)
            {
                List<object> st = codingData[0];

                codingProblem = new VmCodingProblem
                {
                    Instructions = (st[0] != DBNull.Value)? st[0].ToString():String.Empty,
                    Script = (st[1] != DBNull.Value) ? st[1].ToString() : String.Empty,
                    Solution = (st[2] != DBNull.Value) ? st[2].ToString() : String.Empty,
                    ClassName = (st[3] != DBNull.Value) ? st[3].ToString() : String.Empty,
                    MethodName = (st[4] != DBNull.Value) ? st[4].ToString() : String.Empty,
                    ParameterTypes = (st[5] != DBNull.Value) ? st[5].ToString() : String.Empty,
                    Language = (st[6] != DBNull.Value) ? st[6].ToString() : String.Empty,
                    TestCaseClass = (st[7] != DBNull.Value) ? st[7].ToString() : String.Empty,
                    Before = (st[8] != DBNull.Value) ? st[8].ToString() : String.Empty,
                    After = (st[9] != DBNull.Value) ? st[9].ToString() : String.Empty,
                    MaxGrade = (st[10] != DBNull.Value) ? (int)st[10] : 0,
                    Title = (st[11] != DBNull.Value) ? st[11].ToString() : String.Empty,
                    Type = (st[12] != DBNull.Value) ? st[12].ToString() : String.Empty,
                    Attempts = (st[13] != DBNull.Value) ? (int)st[13] : 0,
                    Active = (st[14] != DBNull.Value) ? (bool)st[14] : false,
                    Role = (st[15] != DBNull.Value) ? (int)st[15] : 0,
                    Id = (st[16] != DBNull.Value) ? (int)st[16] : 0,
                    ExpectedOutput = (st[17] != DBNull.Value) ? st[17].ToString() : String.Empty,
                    Parameters = (st[18] != DBNull.Value) ? st[18].ToString() : String.Empty,
                    TestCode = (st[19] != DBNull.Value) ? st[19].ToString() : String.Empty,
                    TestCodeForStudent = (st[20] != DBNull.Value) ? st[20].ToString() : String.Empty
                };
            
            }
            return codingProblem;
        }
        public static List<VmCodingProblemInstance> GetCodingProblemInstance(int codingProblemId, int studentId)
        {
            string sqlQueryCoding = $@"select * from CodingProblemInstance where 
                                    CodingProblemId = {codingProblemId} and StudentId = {studentId}";

            var codingData = SQLHelper.RunSqlQuery(sqlQueryCoding);
            List<VmCodingProblemInstance> codingProblem = new List<VmCodingProblemInstance>();

            if (codingData.Count > 0)
            {
                foreach (var st in codingData)
                {
                    VmCodingProblemInstance codingProb = new VmCodingProblemInstance
                    {
                        idCodingProblemInstance = (int)st[0],
                        CodingProblemId = (int)st[1],
                        StudentId = (int)st[2],
                        VarName = (string)st[3],
                        VarValue = (string)st[4],
                        idVariableValue = string.IsNullOrEmpty(st[5].ToString()) ? null : (int?)st[5],
                        occurrenceNumber = string.IsNullOrEmpty(st[6].ToString()) ? null : (int?)st[6]
                    };
                    codingProblem.Add(codingProb);
                }                
            }
            return codingProblem;
        }
        
        public static List<VmVariableValue> GetVmVariableValue(int codingProblemId)
        {
            string sqlQueryCoding = $@"select * from VariableValue where 
                                    CodingProblemId = {codingProblemId}";

            var codingData = SQLHelper.RunSqlQuery(sqlQueryCoding);
            List<VmVariableValue> codingProblem = new List<VmVariableValue>();

            if (codingData.Count > 0)
            {
                foreach (var st in codingData)
                {
                    VmVariableValue codingProb = new VmVariableValue
                    {
                        idVariableValue = (int)st[0],
                        CodingProblemId = (int)st[1],                        
                        VarName = (string)st[2],
                        possibleValues = (string)st[3]
                    };
                    codingProblem.Add(codingProb);
                }
            }
            return codingProblem;
        }

        /// <summary>
        /// Searches for no memory instance variables in a CodingProblem's
        /// instructions, solution, test code and expected output.
        /// </summary>
        /// <param name="cp"></param>
        /// <returns name="result">Tuples with variable names and the occurence number, if is single instance the occurence number it's null</returns>
        private static HashSet<(string varName, string occurenceNumber)> GetInstancesWithoutMemory(VmCodingProblem cp)
        {
            var result = new HashSet<(string varName, string occurenceNumber)>();
            foreach (String str in new String[] { cp.Instructions, cp.Solution, cp.TestCode, cp.ExpectedOutput })
            {
                String[] split = Regex.Split(str, @"(\${ *{[^$]*? *} *})");
                for (int i = 0; i < split.Length; ++i)
                {
                    String substr = split[i];
			
                    if (Regex.IsMatch(substr, @"(\${ *{[^$]*? *} *})"))
                    {
                        if (Regex.IsMatch(substr, @"(\${ *{[^$,]*, *\d\d* *} *})"))
                        {
                            //Asumes the var name doesn't include comma, brackets or '$'
                            Regex lineSplitter = new Regex(@"\${ *{ *|,| *} *}");
                            String[] subsplit = lineSplitter.Split(substr).Where(s => s != String.Empty).ToArray();
                            String varName = Regex.Replace(subsplit[0], @"(\$)|({)|(})|( )|(\[\d*\])", "");
                            String occurenceNumber = Regex.Replace(subsplit[1], @"(\$) | ({)| (})| ( ) | (\[)| (\])", "");
			    VmVariableValue varValue = GetVariableValuesId(cp.Id, varName);
                            //if (cp.VariableValues.Where(v => v.VarName == varName).Any())
			    if (varValue != null)
                            {
                                if (!result.Any(x => x.varName.Equals(varName) && x.occurenceNumber == occurenceNumber))
                                {
                                    result.Add((varName, occurenceNumber));
                                }
                            }
                        }
                        else
                        {
                            String varName = Regex.Replace(substr, @"(\$)|({)|(})|( )|(\[\d*\])", "");
                            //String[] varNamePossibleTuple = Regex.Split(varName, @"(?=[[])");
			    VmVariableValue varValue = GetVariableValuesId(cp.Id, varName);
                            //if (cp.VariableValues.Where(v => v.VarName == varName).Any())
		            if (varValue != null)
                            {
                                result.Add((varName, null));
                            }
                        }
                    }
                }
            }
            return result;
        }
        private static HashSet<(string varName, string varValue, string ocurrenceNumber)> GetInstancesTuples(VmCodingProblem cp, HashSet<(string varName, string varValue, string ocurrenceNumber)> setForValidation)
        {
            var result = new HashSet<(string varName, string varValue, string ocurrenceNumber)>();
            foreach (String str in new String[] { cp.Instructions, cp.Solution, cp.TestCode, cp.ExpectedOutput })
            {
                var split = Regex.Matches(str, @"\$\{((?:\{[^\$]*?(\[\d+]|\[\d+],\d)\}|[^\$]*?(\[\d+]|\[\d+],\d)))\}");
                for (int i = 0; i < split.Count; ++i)
                {
                    String ocurrenceNumber = null;
                    String substr = split[i].ToString();
                    String varName = Regex.Replace(substr, @"(\$)|({)|(})|( )", "");
                    String[] varNamePossibleTuple = Regex.Split(varName, @"(?=[[])");
		    VmVariableValue varValue = GetVariableValuesId(cp.Id, varName);
                    //if (cp.VariableValues.Where(v => v.VarName == varNamePossibleTuple[0]).Any() && result.Where(v => v.varName == varName).Count() == 0)
		    if (varValue != null && result.Where(v => v.varName == varName).Count() == 0)
                    {
                        varNamePossibleTuple[1] = Regex.Replace(varNamePossibleTuple[1], @"(\$)|(\[)|(\])|( )", "");
                        String[] varNamePossibleMultiInstance = Regex.Split(varNamePossibleTuple[1], @"(?=[,])");

                        string value;

                        if (substr.Count(f => f == ',') > 0)
                        {
                            varNamePossibleMultiInstance[1] = Regex.Replace(varNamePossibleMultiInstance[1], @"(,)", "");

                            value = setForValidation.Where(cpi => cpi.varName == varNamePossibleTuple[0] && cpi.ocurrenceNumber != null && cpi.ocurrenceNumber.ToString() == varNamePossibleMultiInstance[1]).First().varValue;
                            ocurrenceNumber = varNamePossibleMultiInstance[1];
                        }
                        else
                        {
                            value = setForValidation.Where(cpi => cpi.varName == varNamePossibleTuple[0]).First().varValue;
                        }
                        if (value.Contains("["))
                        {
                            string[] values = value.TrimStart('[').TrimEnd(']').Split(',');
                            value = values[int.Parse(varNamePossibleMultiInstance[0])];
                        }
                        else
                        {
                            string[] values = value.TrimStart('{').TrimEnd('}').Split(',');
                            value = values[int.Parse(varNamePossibleTuple[0])];
                        }
                        varName = Regex.Replace(varName, @"(,\d*)", "");
                        result.Add((varName, value, ocurrenceNumber));
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// Searches for multiple instance variables in a CodingProblem's
        /// instructions, solution, test code and expected output.
        /// </summary>
        /// <param name="cp"></param>
        /// <returns name="result">Tuples with variable names and their instances</returns>
        private static (String varName, String[] instances)[] GetMultipleInstances(VmCodingProblem cp)
        {
            var result = new Dictionary<String, (String, String[])>();
            foreach (String str in new String[] { cp.Instructions, cp.Solution, cp.TestCode, cp.ExpectedOutput })
            {
                String[] split = Regex.Split(str, @"(\${[^$,]*?, *\d\d* *?})");
                for (int i = 0; i < split.Length; ++i)
                {
                    String substr = split[i];
                    if (Regex.IsMatch(substr, @"(\${[^$,]*, *\d\d* *})"))
                    {
                        //Asumes the var name doesn't include comma, brackets or '$'
                        Regex lineSplitter = new Regex(@"\${|,|}");
                        String[] subsplit = lineSplitter.Split(substr).Where(s => s != String.Empty).ToArray();
                        subsplit[0] = Regex.Replace(subsplit[0], @"(\$)|({)|(})|( )|(\[\d*\])", "");
                        subsplit[1] = Regex.Replace(subsplit[1], @"(\$) | ({)| (})| ( ) | (\[)| (\])", "");
			VmVariableValue varValue = GetVariableValuesId(cp.Id, subsplit[0]);
                        //if (cp.VariableValues.Where(v => v.VarName == subsplit[0]).Any())
			if (varValue != null)
                        {
                            if (!result.ContainsKey(subsplit[0]))
                            {
                                result.Add(subsplit[0], (subsplit[0], new String[] { subsplit[1] }));
                            }
                            else if (!result[subsplit[0]].Item2.Contains(subsplit[1]))
                            {
                                //concatenates previous instances with the new found
                                String[] newInstances = new String[result[subsplit[0]].Item2.Length + 1];
                                result[subsplit[0]].Item2.CopyTo(newInstances, 0);
                                newInstances[result[subsplit[0]].Item2.Length] = subsplit[1];
                                result[subsplit[0]] = (subsplit[0], newInstances);
                            }
                        }
                    }
                }
            }
            return result.Values.ToArray();
        }

        public static object GetCodingProblem(string hash, int codingProblemId, int courseInstanceId)
        {
            VmLoadAllDataInfo allDataInfo = LoadAllData(hash, codingProblemId);
            var codingProblemData = allDataInfo.CodingProblem;
            var studentInfo = allDataInfo.StudentInfo;
            VmCodingProblemAllInfo codingProblemAllInfo = GetAllCodingProblemData(studentInfo.StudentId, codingProblemId, courseInstanceId);
            var lastSubmissionData = codingProblemAllInfo.Submission;
            var studentGradableData = codingProblemAllInfo.StudentGradeable;

            string last = codingProblemData.Script;
            string grade = "0%";
            string comment = "";
            if (lastSubmissionData != null)
            {
                if (!string.IsNullOrWhiteSpace(lastSubmissionData.Comment))
                {
                    comment = lastSubmissionData.Comment;
                }
                last = lastSubmissionData.Code;
                if (studentGradableData != null)
                {
                    grade = studentGradableData.Grade + "%";
                }
            }

            int submissions = 0;
            if (codingProblemData.Type == "code")
            submissions = codingProblemAllInfo.totalSubmission;
            bool pastdue = false;
            bool maxreached = false;

            DateTime? dueDate = codingProblemAllInfo.dueDate;
            if (!(studentInfo.Test.HasValue && studentInfo.Test.Value))
            {
                if (dueDate.HasValue &&
                    DateTime.Now > dueDate.Value.AddHours(2))
                {
                    pastdue = false;
                }
                if (submissions >= codingProblemData.Attempts)
                {
                    maxreached = true;
                }
            }
            //Language language = data.Languages.Where(l => l.Name == codingProblem.Language).FirstOrDefault();
            VmLanguage language = GetLanguageInfo(codingProblemData.Language);
            var result = new
            {
                Instructions = allDataInfo.codingProblemInstanceInstructions,
                Script = allDataInfo.codingProblemInstanceScript,
                codingProblemData.Language,
                Solution = allDataInfo.codingProblemInstanceSolution,
                codingProblemData.Attempts,
                last,
                grade,
                submissions,
                pastdue,
                maxreached,
                dueDate,
                now = DateTime.Now.ToString(),
                language.Keywords,
                language.KeywordsOutput,
                codingProblemData.Title,
                comment,
                Before = allDataInfo.codingProblemInstanceBefore,
                After = allDataInfo.codingProblemInstanceAfter,
                TestCodeForStudent = allDataInfo.codingProblemInstanceTestCodeForStudent,
                TestCodeFilename = codingProblemData.TestCodeForStudent != null && codingProblemData.TestCodeForStudent != string.Empty ? GetTestCodeFilenameNew(codingProblemData.TestCodeForStudent, $@"{studentInfo.StudentId}-debug", language) : ""
            };
            return result;
        }

        private static string GetTestCodeFilename(string testCode, string nameFile, VmLanguage language)
        {
            if (language.Name == "Java")
            {
                string find = "public class";
                int index = testCode.IndexOf(find);
                string name1 = testCode.Substring(index + find.Length + 1);
                int index1 = name1.IndexOf(" ");
                string name = name1.Substring(0, index1);
                return $@"{name}.java";
            }
            else
            {
                string testCodeExtension = language.SourceExtension ?? "txt";
                return $@"{nameFile}.{testCodeExtension}";
            }
        }

        private static VmSubmission GetLastSubmission(int codingProblemId)
        {
            if (codingProblem.Type != "code")
                return null;

            VmSubmission vmSubmission = GetSubmission(student.StudentId, codingProblemId);
            return vmSubmission;
        }

        private static VmSubmission GetSubmission(int studentId, int codingProblemId)
        {
            string sqlQueryGradeScale = $@"select * from Submission where StudentId = {studentId} and CodingProblemId = {codingProblemId} order by Id desc";

            var gradeScaleData = SQLHelper.RunSqlQuery(sqlQueryGradeScale);
            VmSubmission vmSubmission = null;

            if (gradeScaleData.Count > 0)
            {
                List<object> st = gradeScaleData[0];

                vmSubmission = new VmSubmission
                {
                    Id = (int)st[0],
                    StudentId = (int)st[1],
                    Code = (string)st[2],
                    TimeStamp = (DateTime)st[3],
                    Grade = st[4] != DBNull.Value ? (int)st[4] :0,
                    History = st[5] != DBNull.Value ? (string)st[5] : String.Empty,
                    CodingProblemId = (int)st[6],
                    CourseInstanceId = st[7] != DBNull.Value ? (int)st[7] : 0,
                    Comment = st[8] != DBNull.Value ? (string)st[8] : String.Empty,
                    Error = st[9] != DBNull.Value ? (string)st[9] : string.Empty
                };
            }

            return vmSubmission;
        }

        private static int GetSubmissionCount(int studentId, int codingProblemId, int courseInstanceId)
        {
            string sqlQueryGradeScale = $@"select * from Submission where StudentId = {studentId} and CodingProblemId = {codingProblemId} and CourseInstanceId = {courseInstanceId} order by Id desc";

            var gradeScaleData = SQLHelper.RunSqlQuery(sqlQueryGradeScale);
            
            return gradeScaleData.Count;
        }
        private static VmCourseInstanceCodingProblem GetCourseInstanceCodingProblem(int courseInstanceId, int codingProblemId)
        {
            string sqlQueryGradeScale = $@"select  * from CourseInstanceCodingProblem where CodingProblemId = {codingProblemId} and CourseInstanceId = {courseInstanceId}";

            var gradeScaleData = SQLHelper.RunSqlQuery(sqlQueryGradeScale);
            VmCourseInstanceCodingProblem vmSubmission = null;

            if (gradeScaleData.Count > 0)
            {
                List<object> st = gradeScaleData[0];

                vmSubmission = new VmCourseInstanceCodingProblem
                {
                    CourseInstanceId = (int)st[0],
                    ModuleObjectiveId = (int)st[1],
                    CodingProblemId = (int)st[2],
                    MaxGrade = (int)st[3],
                    Active = (bool)st[4],
                    DueDate = st[5] != DBNull.Value ? (DateTime)st[5] : DateTime.MinValue
                };
            }

            return vmSubmission;
        }

        private static bool InsertSubmission(VmSubmission vmSubmission)
        {
            string sqlQueryGradeScale = $@"INSERT INTO [dbo].[Submission]
           ([StudentId]
           ,[Code]
            ,[TimeStamp]
           ,[CodingProblemId]
           ,[CourseInstanceId])
     VALUES
           ({vmSubmission.StudentId}
           ,'{vmSubmission.Code}'
           ,GETDATE()
           ,{vmSubmission.CodingProblemId}
           ,{vmSubmission.CourseInstanceId})";

            bool isSucess = SQLHelper.RunSqlUpdate(sqlQueryGradeScale);
            return isSucess;
        }

        private static bool UpdateSubmissionError(string error, int Id)
        {
            string sqlQueryGradeScale = $@"UPDATE [dbo].[Submission] set Error = '{error}' where Id = {Id}";

            bool isSucess = SQLHelper.RunSqlUpdate(sqlQueryGradeScale);
            return isSucess;
        }

        public static ExecutionResult RunCodeForValidation(VmCodingProblem codingProblem, Dictionary<string, string> varPossibleValuePairs)
        {
            if (String.IsNullOrWhiteSpace(codingProblem.Solution))
            {
                throw new SolutionNotFoundException("The problem solution is needed in order to verify it.");
            }

            VmLanguage language = GetLanguageInfo(codingProblem.Language);

            var varValues = varPossibleValuePairs.Select((entry) =>
                new VmVariableValue()
                {
                    VarName = entry.Key,
                    possibleValues = entry.Value
                })
                .ToList();

            Dictionary<string, string> varValuePairs = GetCodingProblemVariables(varValues);
            codingProblem.VariableValues = varValues;
            var multipleInstances = GetMultipleInstances(codingProblem);
            var missingInstances = new HashSet<(string varName, string possibleValues, int numberOfInstances)>();
            foreach (var (varName, instances) in multipleInstances)
            {
                int numberOfInstances = instances.Count();
                string possibleValues = varValues.First(x => x.VarName.Equals(varName)).possibleValues;
                missingInstances.Add((varName, possibleValues, numberOfInstances));
            }


            Dictionary<string, string[]> sortedMultipleInstances = GetCodingProblemVariablesMultiInstance(missingInstances);
            HashSet<VmCodingProblemInstance> newMultiInstances = new HashSet<VmCodingProblemInstance>();
            foreach (var (varName, instances) in multipleInstances)
            {
                var sortedValues = sortedMultipleInstances.First(x => x.Key.Equals(varName)).Value;
                for (int i = 0; i < instances.Length; i++)
                {
                    VmVariableValue varValue = varValues.Where(vv => vv.VarName == varName).FirstOrDefault();
                    VmCodingProblemInstance inst = new VmCodingProblemInstance
                    {
                        Student = student,
                        CodingProblem = codingProblem,
                        VarName = varName,
                        VarValue = sortedValues[i],
                        idVariableValue = varValue.idVariableValue,
                        occurrenceNumber = int.Parse(instances[i])

                    };
                    newMultiInstances.Add(inst);
                }
            }

            foreach (var v in varValuePairs)
            {
                VmVariableValue varValue = varValues.Where(vv => vv.VarName == v.Key).FirstOrDefault();
                VmCodingProblemInstance inst = new VmCodingProblemInstance
                {
                    Student = student,
                    CodingProblem = codingProblem,
                    VarName = v.Key,
                    VarValue = v.Value,
                    idVariableValue = varValue.idVariableValue,
                    occurrenceNumber = null

                };
                newMultiInstances.Add(inst);

            }

            var instancesData = new HashSet<(string varName, string varValue, string ocurrenceNumber)>();
            foreach (var v in newMultiInstances)
            {
                if (v.occurrenceNumber != null)
                {
                    instancesData.Add((v.VarName, v.VarValue, v.occurrenceNumber.ToString()));
                }
                else
                {
                    instancesData.Add((v.VarName, v.VarValue, null));
                }
            }

            if (instancesData.Count > 0)
            {
                var tuples = GetInstancesTuples(codingProblem, instancesData);
                instancesData.UnionWith(tuples);
                codingProblem.TestCode = SharedFunctions.InitializeVariablesInString(codingProblem.TestCode, instancesData, "NonStudent");
                codingProblem.ExpectedOutput = SharedFunctions.InitializeVariablesInString(codingProblem.ExpectedOutput, instancesData, "NonStudent");
                codingProblem.Solution = SharedFunctions.InitializeVariablesInString(codingProblem.Solution, instancesData, "NonStudent");
                codingProblem.Before = SharedFunctions.InitializeVariablesInString(codingProblem.Before, instancesData, "NonStudent");
                codingProblem.After = SharedFunctions.InitializeVariablesInString(codingProblem.After, instancesData, "NonStudent");
                codingProblem.Script = SharedFunctions.InitializeVariablesInString(codingProblem.Script, instancesData, "NonStudent");
            }

            RunInfo runInfo = new RunInfo
            {
                Language = codingProblem.Language,
                Path = HttpRuntime.AppDomainAppPath,
                StudentId = "NonStudent",
                Script = codingProblem.Script,
                Code = GetSolutionAsAnwer(codingProblem.Solution, codingProblem.Script, language),
                Test = codingProblem.TestCode,
                Before = codingProblem.Before,
                After = codingProblem.After,
                ClassName = "",
                MethodName = codingProblem.MethodName,
                ParameterTypes = codingProblem.ParameterTypes,
                Parameters = language.CompilationParameters,
                ExpectedResult = codingProblem.ExpectedOutput,
                Dependencies = new List<string>(),
                Solution = codingProblem.Solution
            };
            CompilerInfo compilerInfo = new CompilerInfo
            {
                CodeEnd = language.CodeEnd,
                CodeStart = language.CodeStart,
                Comment = language.Comment,
                Compiler = language.Compiler,
                CompilerDirectory = language.CompilerDirectory,
                OutputExtension = language.OutputExtension,
                SourceExtension = language.SourceExtension,
                TestToolDirectory = language.TestToolDirectory,
                TestToolExe = language.TestToolExe,
                CompilationParameters = language.CompilationParameters
                
            };

            return CompilerTemp.RunCodeForValidation(runInfo, compilerInfo);
        }
        private static string GetSolutionAsAnwer(string solution, string script, VmLanguage codingProblemLanguage)
        {
            if (!IsFileUploadProblem(codingProblemLanguage.Name)
                && codingProblemLanguage.Name != "Python"
                && codingProblemLanguage.Name != "SQL"
                && codingProblemLanguage.Name != "AzureDO"
                && codingProblemLanguage.Name != "R"
                && codingProblemLanguage.Name != "Cpp"
                && codingProblemLanguage.Name != "DB"
                && codingProblemLanguage.Name != "Browser"
                && codingProblemLanguage.Name != "CosmoDB"
                && codingProblemLanguage.Name != "WebVisitor"
            )
            {
                var solutionClassName = GetClassName(solution, codingProblemLanguage);
                var scriptClassName = GetClassName(script, codingProblemLanguage);
                var answer = solution.Replace(solutionClassName, scriptClassName);

                int functionStart = script.IndexOf(scriptClassName);
                int functionStartAnswer = answer.IndexOf(scriptClassName);

                var newAnswer = script.Substring(0, functionStart) + answer.Substring(functionStartAnswer);
                return newAnswer;

            }
            return solution;
        }
        private static string GetClassName(string code, VmLanguage codingProblemLanguage)
        {
            string pattern = GetPattern(codingProblemLanguage);
            if (code == null)
            {
                return code;
            }
            Regex rgx = new Regex(pattern);
            var match = rgx.Match(code);
            if (!match.Success)
            {
                throw new ClassOrFunctionNotFoundException("Class name or function not found in: " + code);
            }
            return match.Groups["token"].Value;
        }

        private static string GetPattern(VmLanguage codingProblemLanguage)
        {
            switch (codingProblemLanguage.Name)
            {
                case "C#":
                case "Cpp":
                case "Java": return @"class +(?<token>\w+)";
                case "Python": return @"def +(?<token>\w+) *\(";
                case "R": return @"(?<token>\w+) *<- *function";
                default: return null;
            }
        }
        private static VmSanitizedCodeError GetErrorParsings(string error)
        {
            string sqlStudentCourse = $@"select top 1 * from SanitizedCodeError where Error = '{error}'";

            var studentGradableData = SQLHelper.RunSqlQuery(sqlStudentCourse);
            VmSanitizedCodeError vmSanitizedCode = null;

            if (studentGradableData.Count > 0)
            {
                List<object> st = studentGradableData[0];

                vmSanitizedCode = new VmSanitizedCodeError
                {
                    Id = (int)st[0],
                    Error = st[1].ToString()
                };
            }
            return vmSanitizedCode;
        }
        private static string[] GetErrors(string language)
        {
            string sqlStudentCourse = $@"select ep.ErrorPattern from ErrorParsing as ep
inner join Language as la on ep.LanguageId = la.Id
inner join CodingProblem as cp on la.Name = cp.Language
where cp.Language = '{language}'
order by ep.PatternOrder asc";

            var studentGradableData = SQLHelper.RunSqlQuery(sqlStudentCourse);
            List<string> errors = new List<string>();

            if (studentGradableData.Count > 0)
            {
                foreach (var item in studentGradableData)
                {
                    string error = (string)item[0];
                    errors.Add(error);
                }
            }
            return errors.ToArray();
        }
        private static bool InsertSanitizedCodeError(VmSanitizedCodeError vmSubmission)
        {
            string sqlQueryGradeScale = $@"INSERT INTO [dbo].[SanitizedCodeError]
           ([Error])
     VALUES
           ('{vmSubmission.Error}')";

            bool isSucess = SQLHelper.RunSqlUpdate(sqlQueryGradeScale);
            return isSucess;
        }
        private static bool InsertCodeError(VmCodeError vmSubmission)
        {
            string sqlQueryGradeScale = $@"INSERT INTO [dbo].[CodeError]
           ([Error]
           ,[SanitizedErrorId])
     VALUES
           ('{vmSubmission.Error}'
           ,{vmSubmission.SanitizedErrorId})";

            bool isSucess = SQLHelper.RunSqlUpdate(sqlQueryGradeScale);
            return isSucess;
        }
        private static bool InsertSubmissionCodeError(VmSubmissionCodeError vmSubmission)
        {
            string sqlQueryGradeScale = $@"INSERT INTO [dbo].[SubmissionCodeError]
           ([CodeErrorId]
           ,[SubmissionId])
     VALUES
           ({vmSubmission.CodeErrorId}
           ,{vmSubmission.SubmissionId})";

            bool isSucess = SQLHelper.RunSqlUpdate(sqlQueryGradeScale);
            return isSucess;
        }

        private static bool InsertSubmissionCodeErrorStoreProcedure(string error, int submissionId)
        {
            List<Param> paramss = new List<Param>();
            paramss.Add(new Param()
            {
                Value = error,
                Name = "error"
            });
            paramss.Add(new Param()
            {
                Value = submissionId.ToString(),
                Name = "submissionId"
            });

            bool isSucess = SQLHelper.RunSqlUpdateWithParam("AddCodeSubmissionError", paramss);
            return isSucess;
        }
        private static void AddCodeError(string rawError, VmSubmission submission)
        {
            //Separate multiple errors into list
            string[] errorList = rawError.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            string[] patterns = GetErrors(codingProblem.Language);

            foreach (string error in errorList)
            {
                string sanitizedError = error;

                foreach (string pattern in patterns)
                {
                    sanitizedError = Regex.Replace(sanitizedError, pattern, "");
                }
                //Sanitize the error
                //Get sanitized error from database
                VmSanitizedCodeError sanitizedCodeError = GetErrorParsings(sanitizedError);

                //Create new sanitized error if one doesn't exist
                if (sanitizedCodeError == null)
                {
                    sanitizedCodeError = new VmSanitizedCodeError
                    {
                        Error = sanitizedError
                    };
                    InsertSubmissionCodeErrorStoreProcedure(error, submission.Id);
                }

                else
                { //Create nonsanitized error
                    VmCodeError codeError = new VmCodeError
                    {
                        Error = error,
                        SanitizedErrorId = sanitizedCodeError.Id
                    };


                    //codeError = data.CodeErrors.Add(codeError);
                    InsertCodeError(codeError);
                    //Add nonsanitized error and submission to join table
                    VmSubmissionCodeError submissionCodeError = new VmSubmissionCodeError
                    {
                        CodeErrorId = codeError.Id,
                        SubmissionId = submission.Id,
                    };
                    //data.SubmissionCodeErrors.Add(submissionCodeError);
                    InsertSubmissionCodeError(submissionCodeError);
                    //data.SaveChanges();

                }

            }
        }

        private static List<VmCodeError> GetCodeErrors(int submissionId)
        {
            string sqlStudentCourse = $@"select ce.* from SubmissionCodeError as sc
inner join CodeError as ce on sc.CodeErrorId = ce.Id
where SubmissionId = {submissionId}";

            var studentGradableData = SQLHelper.RunSqlQuery(sqlStudentCourse);
            List<VmCodeError> errors = new List<VmCodeError>();

            if (studentGradableData.Count > 0)
            {
                foreach (var item in studentGradableData)
                {
                    var vmCodeError = new VmCodeError()
                    {
                        Id = (int)item[0],
                        Error = (string)item[1],
                        SanitizedErrorId = (int)item[2]
                    };
                    errors.Add(vmCodeError);
                }
            }
            return errors;
        }
        private static string GetCodeErrorHint(int submissionId)
        {
            string sqlStudentCourse = $@"select top 1 ch.Hint from SanitizedCodeErrorCodeHint as se
inner join CodeHint as ch on se.CodeHintId = ch.Id
where se.SanitizedCodeErrorId = {submissionId}";

            var studentGradableData = SQLHelper.RunSqlQuery(sqlStudentCourse);
            string hint = null;

            if (studentGradableData.Count > 0)
            {
                List<object> st = studentGradableData[0];
                hint = (string)st[0];
            }
            return hint;
        }

        private static VmLanguage GetLanguage(string name)
        {
            string sqlStudentCourse = $@"select * from Language where Name = '{name}'";

            var studentGradableData = SQLHelper.RunSqlQuery(sqlStudentCourse);
            VmLanguage vmLanguage = null;

            if (studentGradableData.Count > 0)
            {
                List<object> st = studentGradableData[0];
                vmLanguage = new VmLanguage()
                {
                    Id = (int)st[0],
                    Name = st[1] != DBNull.Value ? (string)st[1] : String.Empty,
                    Keywords = st[2] != DBNull.Value ? (string)st[2] : String.Empty,
                    KeywordsOutput = st[3] != DBNull.Value ? (string)st[3] : String.Empty,
                    Comment = st[4] != DBNull.Value ? (string)st[4] : String.Empty,
                    CodeStart = st[5] != DBNull.Value ? (string)st[5] : String.Empty,
                    CodeEnd = st[6] != DBNull.Value ? (string)st[6] : String.Empty,
                    CompilerDirectory = st[7] != DBNull.Value ? (string)st[7] : String.Empty,
                    SourceExtension = st[8] != DBNull.Value ? (string)st[8] : String.Empty,
                    Compiler = st[9] != DBNull.Value ? (string)st[9] : String.Empty,
                    OutputExtension = st[10] != DBNull.Value ? (string)st[10] : String.Empty,
                    CompilationParameters = st[11] != DBNull.Value ? (string)st[11] : String.Empty,
                    TestToolDirectory = st[12] != DBNull.Value ? (string)st[12] : String.Empty,
                    TestToolExe = st[13] != DBNull.Value ? (string)st[13] : String.Empty,
                    OutParameters = st[14] != DBNull.Value ? (string)st[14] : String.Empty
                };
            }
            return vmLanguage;
        }

        private static IEnumerable<CodeHint> GetHints(VmSubmission submission)
        {
            List<CodeHint> codeHints = new List<CodeHint>();

            //Get all code errors
            List<VmCodeError> codeErrors = GetCodeErrors(submission.Id);
            foreach (VmCodeError codeError in codeErrors)
            {
                string hint = GetCodeErrorHint(codeError.SanitizedErrorId);
                if (hint == null)
                {
                    hint = "No hints available. Contact instructor.";
                }

                CodeHint codeHint = new CodeHint
                {
                    Error = codeError.Error,
                    Hint = hint,
                };

                codeHints.Add(codeHint);
            }

            return codeHints;
        }

        public static object RunCode(string hash, int codingProblemId, int courseInstanceId, string answer, int codeStructurePonits)
        {
            Load(hash, codingProblemId);

            string studentId = GetStudentId();
            VmSubmission lastSubmission = GetLastSubmission(codingProblemId);

            if (!string.IsNullOrEmpty(answer)
                && answer != codingProblem.Script
                && (lastSubmission == null || answer != lastSubmission.Code))
            {
                VmSubmission submission = new VmSubmission
                {
                    CodingProblemId = codingProblem.Id,
                    StudentId = student.StudentId,
                    Code = answer,
                    TimeStamp = DateTime.Now,
                    //TODO: Marcelo
                    CourseInstanceId = courseInstanceId
                    //CourseInstanceId = -1
                };
                InsertSubmission(submission);
            }

            //grade weights
            double compilationWeight;
            double testsWeight;
            double codeWeight;
            if (IsFileUploadProblem(codingProblem.Language))
            {
                compilationWeight = 0.0;
                testsWeight = 1.0;
                codeWeight = 0.0;
            }
            else
            {
                compilationWeight = 0.2;
                testsWeight = 0.8;
                codeWeight = 0.0;
            }

            int totalScore = 0;

            bool pastdue = false;
            bool maxreached = false;

            DateTime? dueDate = GetCourseInstanceCodingProblem(courseInstanceId, codingProblemId).DueDate;
            int submissions = GetSubmissionCount(student.StudentId, codingProblemId, courseInstanceId);

            if (!(student.Test.HasValue && student.Test.Value))
            {
                if (dueDate.HasValue &&
                    DateTime.Now > dueDate.Value.AddHours(2))
                {
                    pastdue = true;
                }
                if (submissions >= codingProblem.Attempts)
                {
                    maxreached = true;
                }
            }

            ResultInfo ri = new ResultInfo
            {
                Attempts = codingProblem.Attempts,
                Submissions = submissions,
                DueDate = dueDate.ToString(),
                Now = DateTime.Now.ToString(),
                MaxReached = maxreached,
                PastDue = pastdue,
            };

            List<TestResult> testResults = new List<TestResult>();
            int testPassed = 0;
            TestResult testCase = new TestResult();
            string[] dependencies = Array.Empty<string>();

            testCase.Expected = codingProblemInstanceExpectedOutput;

            string before = codingProblemInstanceBefore ?? "";
            string after = codingProblemInstanceAfter ?? "";

            VmLanguage language = GetLanguage(codingProblem.Language);
            RunInfo runInfo = new RunInfo
            {
                Language = codingProblem.Language,
                Path = HttpRuntime.AppDomainAppPath,
                StudentId = studentId,
                Script = codingProblem.Script,
                Code = answer,
                Test = codingProblemInstanceTestCode,
                Before = codingProblemInstanceBefore,
                After = codingProblemInstanceAfter,
                ClassName = codingProblemInstanceTestCode,
                MethodName = codingProblem.MethodName,
                ParameterTypes = codingProblem.ParameterTypes,
                Parameters = language.CompilationParameters,
                ExpectedResult = codingProblemInstanceExpectedOutput,
                Dependencies = new List<string>(),
                Solution = codingProblemInstanceSolution
            };
            CompilerInfo compilerInfo = new CompilerInfo
            {
                CodeEnd = language.CodeEnd,
                CodeStart = language.CodeStart,
                Comment = language.Comment,
                Compiler = language.Compiler,
                CompilerDirectory = language.CompilerDirectory,
                OutputExtension = language.OutputExtension,
                SourceExtension = language.SourceExtension,
                TestToolDirectory = language.TestToolDirectory,
                TestToolExe = language.TestToolExe,
                CompilationParameters = language.CompilationParameters,
                OutParameters = language.OutParameters
            };
            ri.ExeResult = CompilerTemp.Run(runInfo, compilerInfo, instancesData, studentId);

            if (ri.ExeResult.Compiled == false)
            {
                VmSubmission currentSubmission = GetLastSubmission(codingProblemId);
                if (currentSubmission != null)
                {
                    currentSubmission.Error = ri.ExeResult.Message[0];
                    //data.SaveChanges();//Today
                    UpdateSubmissionError(ri.ExeResult.Message[0], currentSubmission.Id);
                    if (lastSubmission == null || currentSubmission.Id != lastSubmission.Id)
                    {
                        AddCodeError(ri.ExeResult.Message[0], currentSubmission);
                    }
                    ri.CodeHints = GetHints(currentSubmission);
                }
            }

            testCase.ActualErrors = ri.ExeResult.ActualErrors;
            testCase.Actual = ri.ExeResult.Output;
            if (testCase.Expected == "")
            {
                testCase.Expected = ri.ExeResult.Expected;
            }

            if (ri.ExeResult.Grade <= 0)
            {
                testCase.Result = "Failed";
                testCase.Passed = false;
            }
            else if (ri.ExeResult.Grade == 100)
            {
                totalScore += ri.ExeResult.Grade;
                testCase.Result = "Correct";
                testCase.Passed = true;
                testPassed++;
            }
            else if (ri.ExeResult.Grade > 0)
            {
                totalScore += ri.ExeResult.Grade;
                testCase.Result = "Partial: " + ri.ExeResult.Grade + "%";
                testCase.Passed = false;
            }

            testResults.Add(testCase);

            int finalGrade = totalScore;

            //weighted grade calculation
            int compilationGrade = ri.ExeResult.Compiled ? 100 : 0;

            int structureGrade = 0;
            string codeResult = "";

            if (!IsFileUploadProblem(codingProblem.Language))
            {
                codeResult = (codeStructurePonits < 0) ? (codeStructurePonits + " points") : "good";
                if (codeStructurePonits >= 0)
                {
                    structureGrade = 100;
                }
                else if (codeStructurePonits <= -10)
                {
                    structureGrade = 0;
                }
                else
                {
                    structureGrade = (10 + codeStructurePonits) * 10;
                }
            }
            int totalGrade = (int)Math.Round(compilationGrade * compilationWeight + finalGrade * testsWeight + structureGrade * codeWeight);

            GradeInfo gi = new GradeInfo()
            {
                CompilationWeight = (int)(compilationWeight * 100),
                CompilationGrade = (int)Math.Round(compilationGrade * compilationWeight),
                TestsWeight = (int)(testsWeight * 100),
                TestsGrade = (int)Math.Round(finalGrade * testsWeight),
                CodeWeight = (int)(codeWeight * 100),
                CodeGrade = (int)Math.Round(structureGrade * codeWeight),
                CodeResult = codeResult,
                TotalGrade = totalGrade
            };

            //StudentGradable studentGradable = student.StudentGradables.Where(sg => sg.CodingProblemId == codingProblem.Id && sg.CourseInstanceId == courseInstanceId).FirstOrDefault();
            VmStudentGradeable studentGradable = GetStudentGradable(codingProblem.Id, courseInstanceId);
            if (studentGradable == null)
            {
                string sqlQuery = $@"INSERT INTO [dbo].[StudentGradable]
           ([StudentId]
           ,[Grade]
           ,[MaxGrade]
           ,[CodingProblemId]
           ,[CourseInstanceId])
     VALUES
           ({student.StudentId}
           ,{totalGrade}
           ,{codingProblem.MaxGrade}
           ,{codingProblem.Id}
           ,{courseInstanceId})";

                var data = SQLHelper.RunSqlUpdate(sqlQuery);
            }
            else
            {
                if (totalGrade > studentGradable.Grade)
                {
                    //studentGradable.Grade = totalGrade;
                    string sqlQuery = $@"UPDATE [dbo].[StudentGradable] set Grade = {totalGrade} 
                                        where ID = {studentGradable.Id} ";
                    var data = SQLHelper.RunSqlUpdate(sqlQuery);
                }
                else
                {
                    totalGrade = studentGradable.Grade;
                }
            }

            string pgrade = totalGrade.ToString() + "%";

            // gets the keyword counts

            ri.Tests = testResults;
            ri.BestGrade = pgrade;
            ri.TestPassed = testPassed;
            ri.GradeTable = gi;
            ri.TestCount = 1;
            ri.KeywordCount = GetDiffKeyWordsBetweenSolutionAndAnswer(language.Keywords, answer);
            ri.last = answer;
            return ri;
        } // end of public static object RunCode(string hash, int codingProblemId, int courseInstanceId, string answer, int codeStructurePonits)

        private static VmStudentGradeable GetStudentGradable(int codingProblemId, int courseInstanceId)
        {
            string sqlStudentCourse = $@"select top 1 * from StudentGradable sg 
                                        where sg.CodingProblemId = {codingProblemId} and sg.CourseInstanceId = {courseInstanceId}";




            var studentGradableData = SQLHelper.RunSqlQuery(sqlStudentCourse);
            VmStudentGradeable studentGradable = null;

            if (studentGradableData.Count > 0)
            {
                foreach (var item in studentGradableData)
                {
                    studentGradable = new VmStudentGradeable
                    {
                        StudentId = (int)item[0],
                        Grade = (int)item[1],
                        MaxGrade = (int)item[2],
                        Id = (int)item[3],
                        CodingProblemId = (item[4] ==null)?(int)item[4]:0,
                        CourseInstanceId = (item[5] == null) ? (int)item[5]:0
                    };
                    
                }
            }
            return studentGradable;
        }


        private static Dictionary<string, int> GetDiffKeyWordsBetweenSolutionAndAnswer(string Keywords, string answer)
        {
            string solutionWithImports = codingProblemInstanceSolution;

            if (codingProblemInstanceBefore == null || codingProblemInstanceBefore == "")
            {
                //Language lang = data.Languages.Where(l => l.Name == codingProblem.Language).FirstOrDefault();
                VmLanguage lang = GetLanguageInfo(codingProblem.Language);
                solutionWithImports = GetSolutionAsAnwer(codingProblemInstanceSolution, codingProblemInstanceScript, lang);
            }

            var usedKeywordListInSolution = GetUsedKeywordList(solutionWithImports, Keywords, codingProblem.Language);
            var usedKeywordListInAnwser = GetUsedKeywordList(answer, Keywords, codingProblem.Language);
            var usedKeywordListInSolutionCount = usedKeywordListInSolution
                .GroupBy(x => x)
                .ToDictionary(g => g.Key, g => g.Count());
            var usedKeywordListInAnwserCount = usedKeywordListInAnwser
                .GroupBy(x => x)
                .ToDictionary(g => g.Key, g => -g.Count());
            return usedKeywordListInSolutionCount.Concat(usedKeywordListInAnwserCount)
                .GroupBy(x => x.Key, x => x.Value)
                .ToDictionary(g => g.Key, g => g.Sum())
                .Where(g => g.Value != 0)
                .ToDictionary(g => g.Key, g => g.Value);
        }

        private static bool IsFileUploadProblem(string language)
        {
            return ("Tableau".Equals(language) || "Excel".Equals(language) || "Image".Equals(language) || "HTML".Equals(language));
        } // end of private static bool IsFileUploadProblem(string language) 

        // returns the lines numbers of the first string that do not appear in the second one
        private static List<int> GetDifferentLines(string str1, string str2)
        {
            List<int> res = new List<int>();
            str1 = str1.Replace("\r", "").Replace(" ", "").Replace("\t", "");
            str2 = str2.Replace("\r", "").Replace(" ", "").Replace("\t", "");
            string[] arrStr1 = str1.Split('\n');
            string[] arrStr2 = str2.Split('\n');
            int position = 1;
            foreach (string line in arrStr1)
            {
                bool isInTheString = false;
                foreach (string line2 in arrStr2)
                {
                    if (line.Equals(line2))
                    {
                        isInTheString = true;
                        break;
                    }
                }
                if (!isInTheString)
                {
                    res.Add(position);
                }
                position++;
            }
            return res;
        } // end of private static List<int> GetDifferentLines(string str1, string str2) 

        /// <summary>
        /// Takes the solution and finds out how many keywords are in teh solution then returns an array of the keywords. 
        /// Javascript on frontend then creates a count and stringifies it. 
        /// </summary>
        /// <param name="code"></param>
        /// <param name="language"></param>
        /// <returns name="result"></returns>
        private static string[] GetUsedKeywordList(string code, string RWords, string language)
        {
            RemoveStringLiteralsAndComments(ref code, language);

            RegexOptions options = RegexOptions.Multiline | RegexOptions.Singleline;
            string pattern = ProcessKeyWordsAsPattern(RWords);

            return Regex.Matches(code, pattern, options)
                       .Cast<Match>()
                       .Select(m => m.Value)
                       .ToArray();
        }

        private static string ProcessKeyWordsAsPattern(string keyWords)
        {
            return @"\b(" + keyWords.Replace(",", @"|") + @")\b";
        }

        private static void RemoveStringLiteralsAndComments(ref string code, string language)
        {
            RegexOptions options = RegexOptions.Multiline | RegexOptions.Singleline;
            string commentPatternString = GetCommentPattern(language);
            string stringLiteraltPatternString = GetStringLiteraltPattern(language);
            code = Regex.Replace(code, commentPatternString, "", options);
            code = Regex.Replace(code, stringLiteraltPatternString, "", options);
        }

        private static string GetCommentPattern(string language)
        {
            switch (language)
            {
                case "C#":
                case "Java":
                    string singleLineCommentPattern = @"\/\/.*?$";
                    string multiLineCommentPattern = @"\/\*.*?\*\/";
                    return String.Join("|", singleLineCommentPattern, multiLineCommentPattern);
                case "Python":
                case "R":
                    return @"(#.*?$)";
                case "SQL":
                    singleLineCommentPattern = @"--.*?$";
                    multiLineCommentPattern = @"\/\*.*?\*\/";
                    return String.Join("|", singleLineCommentPattern, multiLineCommentPattern);
            }
            return "";
        }

        private static string GetStringLiteraltPattern(string language)
        {
            switch (language)
            {
                case "C#":
                    string stringVerbatimPattern = @"@""(?:[^""]|"""")*""";
                    string stringLiteralPattern = @"(?<!@)""[^(\n|\r|\r\n)]*?(?<!\\)(\\\\)*""";
                    return String.Join("|", stringLiteralPattern, stringVerbatimPattern);
                case "Java":
                    string oneLineStringLiteralPattern = @"(?<!"")""(?!"")[^(\n|\r|\r\n)]*?(?<!\\)(\\\\)*""";
                    string multiLineStringLiteralPattern = @"("""""".*?(?<!\\)"""""")";
                    return String.Join("|", oneLineStringLiteralPattern, multiLineStringLiteralPattern);
                case "Python":
                    string doubleQuoteOneLineStringLiteralPattern = @"(?<!"")""(?!"")[^(\n|\r|\r\n)]*?(?<!\\)(\\\\)*""";
                    string doubleQuoteMultiLineStringLiteralPattern = @"("""""".*?(?<!\\)"""""")";
                    string singleQuoteOneLineStringLiteralPattern = @"(?<!')'(?!')[^(\n|\r|\r\n)]*?(?<!\\)(\\\\)*'";
                    string singleQuoteMultiLineStringLiteralPattern = @"('''.*?(?<!\\)''')";
                    return String.Join("|", doubleQuoteOneLineStringLiteralPattern, doubleQuoteMultiLineStringLiteralPattern, singleQuoteOneLineStringLiteralPattern, singleQuoteMultiLineStringLiteralPattern);
                case "R":
                    string singleQuoteStringLiteralPattern = @"'.*?(?<!\\)(\\\\)*'";
                    string doubleQuoteStringLiteralPattern = @""".*?(?<!\\)(\\\\)*""";
                    return String.Join("|", singleQuoteStringLiteralPattern, doubleQuoteStringLiteralPattern); ;
                case "SQL":
                    return @"'.*?(?<!\\)(\\\\)*'";
            }
            return "";
        }

        /// <summary>
        /// Returns random values for every variable in its specificed range.
        /// </summary>
        /// <returns name="solution">Pairs of &lt;varName, varValue&gt;</returns>
        private static Dictionary<string, String> GetCodingProblemVariables(ICollection<VmVariableValue> variables)
        {
            var solution = new Dictionary<string, String>();
            int first, last, step, maxValue, aux;
            string[] separatingStrings = { ".." };
            Random rd = new Random();

            foreach (var var in variables)
            {
                //Split by "," but not by "/,"
                string[] separated = Regex.Split(var.possibleValues, @"(?<!\/)\,(?![^[]*]|[^{]*})");
                maxValue = 0;
                var weightedArray = new (String, int)[separated.Length];
                for (int i = 0; i < separated.Length; i++)
                {
                    //Asume that the var doesnt contain a ".." unless it is a number
                    string[] words = separated[i].Split(separatingStrings, System.StringSplitOptions.RemoveEmptyEntries);
                    if (words.Length >= 2)
                    {
                        first = Int32.Parse(words[0]);
                        words = words[1].Split(':');
                        if (words.Length >= 2)
                        {
                            last = Int32.Parse(words[0]);
                            step = Int32.Parse(words[1]);
                            int newStep = Math.Abs(last - first) / step;
                            aux = newStep + 1;
                            step = rd.Next(0, aux) * step;
                            maxValue += aux;
                            weightedArray[i] = ((first + step).ToString(), aux);
                        }
                        else
                        {
                            last = Int32.Parse(words[0]);
                            aux = Math.Abs(last - first) + 1;
                            maxValue += aux;
                            weightedArray[i] = ((rd.Next(first, last + 1)).ToString(), aux);

                        }
                    }
                    else if (words.Length > 0)
                    {
                        //If it's a string, replace any instance of "/," by ","
                        words[0] = words[0].Replace("/,", ",");
                        weightedArray[i] = (words[0], 1);
                        maxValue++;
                    }
                }
                int rand_no = rd.Next(0, maxValue);
                foreach (var element in weightedArray)
                {
                    if (rand_no < element.Item2)
                    {
                        solution.Add(var.VarName, element.Item1);
                        break;
                    }
                    rand_no = rand_no - element.Item2;
                }
            }
            return solution;
        }


        //public static Dictionary<string, String[]> GetCodingProblemVariablesMultiInstance((string varName, string possibleValues, int numberOfInstances)[] variables)
        public static Dictionary<string, String[]> GetCodingProblemVariablesMultiInstance(HashSet<(string varName, string possibleValues, int numberOfInstances)> variables)

        {
            var solution = new Dictionary<string, String[]>();
            int first, maxValue, aux;
            string[] separatingStrings = { ".." };
            Random rd = new Random();

            foreach (var var in variables)
            {
                //Split by "," but not by "/,"
                string[] separated = Regex.Split(var.possibleValues, @"(?<!\/)\,(?![^[]*]|[^{]*})");
                maxValue = 0;
                var weightedArray = new (String first, int? last, int? step, HashSet<int> excludeFromIntegerRanges, int value)[separated.Length];

                for (int i = 0; i < separated.Length; i++)
                {
                    //Asume that the var doesnt contain a ".." unless it is a number
                    string[] words = separated[i].Split(separatingStrings, System.StringSplitOptions.RemoveEmptyEntries);
                    if (words.Length >= 2)
                    {
                        weightedArray[i].first = words[0];
                        words = words[1].Split(':');
                        if (words.Length >= 2)
                        {
                            weightedArray[i].last = Int32.Parse(words[0]);
                            weightedArray[i].step = Int32.Parse(words[1]);
                            int newStep = Math.Abs(weightedArray[i].last.Value - Int32.Parse(weightedArray[i].first)) / weightedArray[i].step.Value;
                            aux = newStep + 1;
                            maxValue += aux;
                            weightedArray[i].value = aux;
                            weightedArray[i].excludeFromIntegerRanges = new HashSet<int>();
                        }
                        else
                        {
                            weightedArray[i].last = Int32.Parse(words[0]);
                            aux = Math.Abs(weightedArray[i].last.Value - Int32.Parse(weightedArray[i].first)) + 1;
                            maxValue += aux;
                            weightedArray[i].value = aux;
                            weightedArray[i].excludeFromIntegerRanges = new HashSet<int>();
                        }
                    }
                    else
                    {
                        //If it's a string, replace any instance of "/," by ","
                        words[0] = words[0].Replace("/,", ",");
                        weightedArray[i] = (words[0], null, null, null, 1);
                        maxValue++;
                    }
                }
                String[] solutionArray = new String[var.numberOfInstances];
                for (int i = 0; i < var.numberOfInstances; i++)
                {
                    int rand_no = rd.Next(0, maxValue);
                    for (int j = 0; j < weightedArray.Length; j++)
                    {
                        if (rand_no < weightedArray[j].value)
                        {
                            if (weightedArray[j].last != null)
                            {
                                if (weightedArray[j].step != null)
                                {
                                    //int rangeValue = Math.Abs(weightedArray[j].last.Value - Int32.Parse(weightedArray[j].first)) / weightedArray[j].step.Value + 1;
                                    int rangeValue = weightedArray[j].value + weightedArray[j].excludeFromIntegerRanges.Count;
                                    var range = Enumerable.Range(0, rangeValue).Where(r => !weightedArray[j].excludeFromIntegerRanges.Contains(r));
                                    int index = rd.Next(0, rangeValue - weightedArray[j].excludeFromIntegerRanges.Count);
                                    solutionArray[i] = (range.ElementAt(index) * weightedArray[j].step.Value + Int32.Parse(weightedArray[j].first)).ToString();
                                    weightedArray[j].excludeFromIntegerRanges.Add(range.ElementAt(index));
                                }
                                else
                                {
                                    var range = Enumerable.Range(Int32.Parse(weightedArray[j].first), Math.Abs(weightedArray[j].last.Value - Int32.Parse(weightedArray[j].first) + 1)).Where(r => !weightedArray[j].excludeFromIntegerRanges.Contains(r));
                                    int index = rd.Next(0, Math.Abs(weightedArray[j].last.Value - Int32.Parse(weightedArray[j].first) + 1) - weightedArray[j].excludeFromIntegerRanges.Count);
                                    solutionArray[i] = range.ElementAt(index).ToString();
                                    weightedArray[j].excludeFromIntegerRanges.Add(range.ElementAt(index));
                                }
                            }
                            else
                            {
                                solutionArray[i] = weightedArray[j].first.ToString();
                            }
                            weightedArray[j].value--;
                            maxValue--;
                            break;
                        }
                        rand_no = rand_no - weightedArray[j].value;
                    }
                }
                if (solutionArray.Any(x => x == null))
                {
                    throw new MultipleInstanceNoValuesLeftExeception(String.Format("There are less possible values than instances required for the variable with name {0}", var.varName));
                }
                solution.Add(var.varName, solutionArray);
            }
            return solution;
        }

        private static HashSet<VmCodingProblemInstance> CreateCodingProblemInstance(ICollection<VmVariableValue> codingProblemVariables, List<string> singleInstanceWithoutMemory)
        {
            Dictionary<string, String> codingProblemInstanceVariables = GetCodingProblemVariables(codingProblemVariables);
            HashSet<VmCodingProblemInstance> solution = new HashSet<VmCodingProblemInstance>();
            foreach (var v in codingProblemInstanceVariables)
            {
                VmVariableValue varValue = GetVariableValuesId(codingProblem.Id, v.Key);
                VmCodingProblemInstance inst = new VmCodingProblemInstance
                {
                    Student = student,
                    CodingProblem = codingProblem,
                    VarName = v.Key,
                    VarValue = v.Value,
                    idVariableValue = varValue.idVariableValue,
                    StudentId = student.StudentId,
                    CodingProblemId = codingProblem.Id

                };
                if (!singleInstanceWithoutMemory.Any(varName => varName.Equals(v.Key)))
                {
                    CodingProblemInstanceInsert(inst);
                }
                solution.Add(inst);
            }
            return solution;

        }

        private static VmVariableValue GetVariableValuesId(int codingProblem, string keyName)
        {
            string sqlQueryStudent = $@"select * from VariableValue where CodingProblemId = {codingProblem} and VarName = '{keyName}'";

            var groupDiscussionData = SQLHelper.RunSqlQuery(sqlQueryStudent);
            VmVariableValue vmVariableValue = null;

            if (groupDiscussionData.Count > 0)
            {
                List<object> st = groupDiscussionData[0];

                vmVariableValue = new VmVariableValue
                {
                    idVariableValue = (int)st[0],
                    CodingProblemId = (int)st[1],
                    VarName = (string)st[2],
                    possibleValues = (string)st[3]
                };
            }

            return vmVariableValue;
        }

        private static bool CodingProblemInstanceInsert(VmCodingProblemInstance vmCodingProblemInstance)
        {
            string sqlQueryCodingProblem = $@"INSERT INTO [dbo].[CodingProblemInstance]
           ([CodingProblemId]
           ,[StudentId]
           ,[VarName]
           ,[VarValue]
           ,[idVariableValue])
     VALUES
           ({vmCodingProblemInstance.CodingProblemId}
           ,{vmCodingProblemInstance.StudentId}
           ,'{vmCodingProblemInstance.VarName}'
           ,'{vmCodingProblemInstance.VarValue}'
           ,{vmCodingProblemInstance.idVariableValue})";

            bool isSucess = SQLHelper.RunSqlUpdate(sqlQueryCodingProblem);
            return isSucess;
        }

        private static HashSet<VmCodingProblemInstance> CreateCodingProblemMultiInstance(HashSet<(string varName, string possibleValues, int numberOfInstances)> missingInstances, (string, string[])[] multipleInstances, HashSet<(string varName, string occurenceNumber)> multipleInstanceWithoutMemory)
        {
            Dictionary<string, string[]> sortedMultipleInstances = GetCodingProblemVariablesMultiInstance(missingInstances);
            HashSet<VmCodingProblemInstance> solution = new HashSet<VmCodingProblemInstance>();
            foreach (var (varName, instances) in multipleInstances)
            {
                if (sortedMultipleInstances.Any(x => x.Key.Equals(varName)))
                {
                    var sortedValues = sortedMultipleInstances.First(x => x.Key.Equals(varName)).Value;
                    for (int i = 0; i < instances.Length; i++)
                    {
                        VmVariableValue varValue = GetVariableValuesId(codingProblem.Id, varName);
                        VmCodingProblemInstance inst = new VmCodingProblemInstance
                        {
                            Student = student,
                            CodingProblem = codingProblem,
                            VarName = varName,
                            VarValue = sortedValues[i],
                            idVariableValue = varValue.idVariableValue,
                            occurrenceNumber = int.Parse(instances[i]),
                            StudentId = student.StudentId,
                            CodingProblemId = codingProblem.Id

                        };
                        //TODO Check for instances with memory instead of without memory
                        if (!multipleInstanceWithoutMemory.Any(x => x.varName.Equals(varName) && x.occurenceNumber == instances[i]))
                        {
                            CodingProblemInstanceInsert(inst);
                        }
                        solution.Add(inst);
                    }
                }
            }
            return solution;
        }

        //Functions for unit tests

        public static (VmCodingProblem, VmStudent, HashSet<VmCodingProblemInstance>) LoadTest(string hash, int codingProblemId)
        {
            Load(hash, codingProblemId);
            List<VmCodingProblemInstance> codingProblemInstances = GetCodingProblemInstance(codingProblemId, student.StudentId);
            
            return (codingProblem, student, codingProblemInstances.ToHashSet());
        }

        public static HashSet<(string varName, string occurenceNumber)> GetInstancesWithoutMemoryTest(VmCodingProblem cp)
        {
            return GetInstancesWithoutMemory(cp);
        }

        public static (String varName, String[] instances)[] GetMultipleInstancesTest(VmCodingProblem cp)
        {
            return GetMultipleInstances(cp);
        }

        public static string InitializeVariablesInStringTest(string str, HashSet<(string varName, string varValue, string ocurrenceNumber)> instancesData, string studentId)
        {
            return SharedFunctions.InitializeVariablesInString(str, instancesData, studentId);
        }

        public static void CreateCodingProblemInstanceTest(ICollection<VmVariableValue> codingProblemVariables)
        {
            CreateCodingProblemInstance(codingProblemVariables, new List<string>());
        }

        public static Dictionary<string, String> GetCodingProblemVariablesTest(ICollection<VmVariableValue> codingProblemVariables)
        {
            return GetCodingProblemVariables(codingProblemVariables);
        }

        public static Dictionary<string, String[]> GetCodingProblemVariablesMultiInstanceTest(HashSet<(string varName, string possibleValues, int numberOfInstances)> variables)
        {
            return GetCodingProblemVariablesMultiInstance(variables);
        }

        public static void SetCodingProblem(VmCodingProblem cd)
        {
            codingProblem = cd;
        }
        //========================================================New SQL Connectin Section====================================================
        private static VmLoadAllDataInfo LoadAllData(string hash, int codingProblemId)
        {
            VmLoadAllDataInfo allDataInfo = GetCodingProblemAndStudentData(codingProblemId, hash);
            var studentInfo = allDataInfo.StudentInfo;
            var codingProblemData = allDataInfo.CodingProblem;
           HashSet<(string, string, string)> instancesData = new HashSet<(string, string, string)>();
            if (codingProblemData.VariableValues.Count() == 0)
            {
                string studentId = GetStudentInfoId(studentInfo.Email);
                allDataInfo.codingProblemInstanceInstructions = SharedFunctions.InitializeVariablesInString(codingProblemData.Instructions, instancesData, studentId);
                allDataInfo.codingProblemInstanceSolution = SharedFunctions.InitializeVariablesInString(codingProblemData.Solution, instancesData, studentId);
                allDataInfo.codingProblemInstanceBefore = SharedFunctions.InitializeVariablesInString(codingProblemData.Before, instancesData, studentId);
                allDataInfo.codingProblemInstanceAfter = SharedFunctions.InitializeVariablesInString(codingProblemData.After, instancesData, studentId);
                allDataInfo.codingProblemInstanceScript = SharedFunctions.InitializeVariablesInString(codingProblemData.Script, instancesData, studentId);
                allDataInfo.codingProblemInstanceTestCodeForStudent = codingProblemData.TestCodeForStudent != null ? SharedFunctions.InitializeVariablesInString(codingProblemData.TestCodeForStudent, instancesData, studentId) : "";
            }
            else
            {
                var codingProblemInstances = GetCodingProblemInstance(codingProblemId);
                HashSet<VmCodingProblemInstance> matchingInstances = codingProblemInstances.Where(ins => ins.StudentId == studentInfo.StudentId).ToHashSet();
                //Looks for possible new variables added in a coding problem update
                HashSet<VmCodingProblemInstance> matchingInstancesWithoutOccurrences = matchingInstances.Where(x => x.occurrenceNumber == null).ToHashSet();
                HashSet<VmVariableValue> missingVars = new HashSet<VmVariableValue>();
                
                foreach (var v in codingProblemData.VariableValues)
                {
                    if (!matchingInstancesWithoutOccurrences.Where(ins => ins.idVariableValue == v.idVariableValue).Any())
                    {
                        missingVars.Add(v);
                    }
                }

                var noMemoryInstances = GetInstancesWithoutMemoryNew(codingProblemData);
                var singleInstanceWithoutMemory = noMemoryInstances.Where(x => x.occurenceNumber == null).Select(x => x.varName).ToList();
                HashSet<VmCodingProblemInstance> newInstances = CreateCodingProblemInstanceNew(missingVars, singleInstanceWithoutMemory, allDataInfo);

                matchingInstances.UnionWith(newInstances);

                var multipleInstances = GetMultipleInstancesNew(codingProblemData);
                //TODO delete variables that are no longer in the coding problem
                HashSet<VmCodingProblemInstance> matchingInstancesWithOccurrences = matchingInstances.Where(x => x.occurrenceNumber != null).ToHashSet();
                var missingInstances = new HashSet<(string varName, string possibleValues, int numberOfInstances)>();
                foreach (var (varName, instances) in multipleInstances)
                {
                    int numberOfInstances = 0;
                    foreach (var instance in instances)
                    {
                        if (!matchingInstancesWithOccurrences.Where(ins => ins.VarName.Equals(varName) && ins.occurrenceNumber == int.Parse(instance)).Any())
                        {
                            numberOfInstances++;
                        }
                    }
                    string possibleValues = codingProblemData.VariableValues.First(x => x.VarName.Equals(varName)).possibleValues;
                    if (numberOfInstances > 0)
                    {
                        missingInstances.Add((varName, possibleValues, numberOfInstances));
                    }
                }

                var multipleInstanceWithoutMemory = noMemoryInstances.Where(x => x.occurenceNumber != null).ToHashSet();
                if (missingInstances.Count > 0)
                {
                    HashSet<VmCodingProblemInstance> newMultiInstances = CreateCodingProblemMultiInstanceNew(missingInstances, multipleInstances, multipleInstanceWithoutMemory, allDataInfo);
                    matchingInstances.UnionWith(newMultiInstances);
                }

                instancesData = new HashSet<(string varName, string varValue, string ocurrenceNumber)>();
                foreach (var v in matchingInstances)
                {
                    var occurrenceNumber = v.occurrenceNumber == null ? null : v.occurrenceNumber.ToString();
                    instancesData.Add((v.VarName, v.VarValue, occurrenceNumber));
                }

                var tuples = GetInstancesTuplesNew(codingProblemData, instancesData);
                instancesData.UnionWith(tuples);
                string studentId = GetStudentInfoId(studentInfo.Email);
                allDataInfo.codingProblemInstanceInstructions = SharedFunctions.InitializeVariablesInString(codingProblemData.Instructions, instancesData, studentId);
                allDataInfo.codingProblemInstanceSolution = SharedFunctions.InitializeVariablesInString(codingProblemData.Solution, instancesData, studentId);
                allDataInfo.codingProblemInstanceTestCode = SharedFunctions.InitializeVariablesInString(codingProblemData.TestCode, instancesData, studentId);
                allDataInfo.codingProblemInstanceExpectedOutput = SharedFunctions.InitializeVariablesInString(codingProblemData.ExpectedOutput, instancesData, studentId);
                allDataInfo.codingProblemInstanceBefore = SharedFunctions.InitializeVariablesInString(codingProblemData.Before, instancesData, studentId);
                allDataInfo.codingProblemInstanceAfter = SharedFunctions.InitializeVariablesInString(codingProblemData.After, instancesData, studentId);
                allDataInfo.codingProblemInstanceScript = SharedFunctions.InitializeVariablesInString(codingProblemData.Script, instancesData, studentId);
                allDataInfo.codingProblemInstanceTestCodeForStudent = codingProblemData.TestCodeForStudent != null ? SharedFunctions.InitializeVariablesInString(codingProblemData.TestCodeForStudent, instancesData, studentId) : "";
            }
            return allDataInfo;
        }
        private static VmCodingProblemAllInfo GetAllCodingProblemData(int studentId, int codingProblemId, int courseInstanceId)
        {
            string sqlQuery = $@"select top 1 sbm.Id SubmissionId, sbm.StudentId, sbm.Code, sbm.TimeStamp, sbm.Grade, 
                                        sbm.History, sbm.CodingProblemId, sbm.CourseInstanceId, sbm.Comment, sbm.Error, 
										sg.Grade, sg.MaxGrade, sg.Id StudentGradableId, sg.CodingProblemId, sg.CourseInstanceId,
										(select count(tsbm.Id)
                                        from Submission tsbm
                                        where tsbm.StudentId = {studentId}
                                        and tsbm.CodingProblemId = {codingProblemId}
                                        and tsbm.CourseInstanceId = {courseInstanceId}) as TotalSubmission,
										(select DueDate from CourseInstanceCodingProblem
											where CodingProblemId = {codingProblemId}
											and CourseInstanceId = {courseInstanceId}) as DueDate
                                        from Student st 
                                        left join Submission sbm on st.StudentId = sbm.StudentId
										left join StudentGradable sg on st.StudentId = sg.StudentId and sg.CodingProblemId = sbm.CodingProblemId
										and sg.CourseInstanceId = {courseInstanceId}

                                        where st.StudentId = {studentId}
                                        and sbm.CodingProblemId = {codingProblemId}
                                        order by sbm.Id Desc";

            var dr = SQLHelper.RunSqlQuery(sqlQuery).FirstOrDefault();
            VmCodingProblemAllInfo codingProblemAllInfo = new VmCodingProblemAllInfo();
            if (dr != null)
            {
                if (dr[0].ToString() != "")
                {
                    codingProblemAllInfo.Submission= new VmSubmission { Id = (int)dr[0], StudentId = (int)dr[1], Code = dr[2].ToString(), TimeStamp = (DateTime)dr[3], Grade = dr[4].ToString() == "" ? 0 : (int)dr[4], History = dr[5].ToString(), CodingProblemId = (int)dr[6], CourseInstanceId = (int)dr[7], Comment = dr[8].ToString(), Error = dr[9].ToString() };
                }
                if (dr[12].ToString() != "")
                {
                    codingProblemAllInfo.StudentGradeable = new VmStudentGradeable
                    {
                        StudentId = (int)dr[1],
                        Grade = (int)dr[10],
                        MaxGrade = (int)dr[11],
                        Id = (int)dr[12],
                        CodingProblemId = (int)dr[13],
                        CourseInstanceId = (int)dr[14]
                    };
                }
                codingProblemAllInfo.totalSubmission = (int)dr[15];
                codingProblemAllInfo.dueDate = dr[16].ToString() == "" ? (DateTime?)null : (DateTime)dr[16];
            }
            return codingProblemAllInfo;
        }
        private static HashSet<(string varName, string occurenceNumber)> GetInstancesWithoutMemoryNew(VmCodingProblem cp)
        {
            var result = new HashSet<(string varName, string occurenceNumber)>();
            foreach (String str in new String[] { cp.Instructions, cp.Solution, cp.TestCode, cp.ExpectedOutput })
            {
                String[] split = Regex.Split(str, @"(\${ *{[^$]*? *} *})");
                for (int i = 0; i < split.Length; ++i)
                {
                    String substr = split[i];
                    if (Regex.IsMatch(substr, @"(\${ *{[^$]*? *} *})"))
                    {
                        if (Regex.IsMatch(substr, @"(\${ *{[^$,]*, *\d\d* *} *})"))
                        {
                            //Asumes the var name doesn't include comma, brackets or '$'
                            Regex lineSplitter = new Regex(@"\${ *{ *|,| *} *}");
                            String[] subsplit = lineSplitter.Split(substr).Where(s => s != String.Empty).ToArray();
                            String varName = Regex.Replace(subsplit[0], @"(\$)|({)|(})|( )|(\[\d*\])", "");
                            String occurenceNumber = Regex.Replace(subsplit[1], @"(\$) | ({)| (})| ( ) | (\[)| (\])", "");
                            if (cp.VariableValues.Where(v => v.VarName == varName).Any())
                            {
                                if (!result.Any(x => x.varName.Equals(varName) && x.occurenceNumber == occurenceNumber))
                                {
                                    result.Add((varName, occurenceNumber));
                                }
                            }
                        }
                        else
                        {
                            String varName = Regex.Replace(substr, @"(\$)|({)|(})|( )|(\[\d*\])", "");

                            if (cp.VariableValues.Where(v => v.VarName == varName).Any())
                            {
                                result.Add((varName, null));
                            }
                        }
                    }
                }
            }
            return result;
        }
        private static (String varName, String[] instances)[] GetMultipleInstancesNew(VmCodingProblem cp)
        {
            var result = new Dictionary<String, (String, String[])>();
            foreach (String str in new String[] { cp.Instructions, cp.Solution, cp.TestCode, cp.ExpectedOutput })
            {
                String[] split = Regex.Split(str, @"(\${[^$,]*?, *\d\d* *?})");
                for (int i = 0; i < split.Length; ++i)
                {
                    String substr = split[i];
                    if (Regex.IsMatch(substr, @"(\${[^$,]*, *\d\d* *})"))
                    {
                        //Asumes the var name doesn't include comma, brackets or '$'
                        Regex lineSplitter = new Regex(@"\${|,|}");
                        String[] subsplit = lineSplitter.Split(substr).Where(s => s != String.Empty).ToArray();
                        subsplit[0] = Regex.Replace(subsplit[0], @"(\$)|({)|(})|( )|(\[\d*\])", "");
                        subsplit[1] = Regex.Replace(subsplit[1], @"(\$) | ({)| (})| ( ) | (\[)| (\])", "");
                        if (cp.VariableValues.Where(v => v.VarName == subsplit[0]).Any())
                        {
                            if (!result.ContainsKey(subsplit[0]))
                            {
                                result.Add(subsplit[0], (subsplit[0], new String[] { subsplit[1] }));
                            }
                            else if (!result[subsplit[0]].Item2.Contains(subsplit[1]))
                            {
                                //concatenates previous instances with the new found
                                String[] newInstances = new String[result[subsplit[0]].Item2.Length + 1];
                                result[subsplit[0]].Item2.CopyTo(newInstances, 0);
                                newInstances[result[subsplit[0]].Item2.Length] = subsplit[1];
                                result[subsplit[0]] = (subsplit[0], newInstances);
                            }
                        }
                    }
                }
            }
            return result.Values.ToArray();
        }
        private static HashSet<(string varName, string varValue, string ocurrenceNumber)> GetInstancesTuplesNew(VmCodingProblem cp, HashSet<(string varName, string varValue, string ocurrenceNumber)> setForValidation)
        {
            var result = new HashSet<(string varName, string varValue, string ocurrenceNumber)>();
            foreach (String str in new String[] { cp.Instructions, cp.Solution, cp.TestCode, cp.ExpectedOutput })
            {
                var split = Regex.Matches(str, @"\$\{((?:\{[^\$]*?(\[\d+]|\[\d+],\d)\}|[^\$]*?(\[\d+]|\[\d+],\d)))\}");
                for (int i = 0; i < split.Count; ++i)
                {
                    String ocurrenceNumber = null;
                    String substr = split[i].ToString();
                    String varName = Regex.Replace(substr, @"(\$)|({)|(})|( )", "");
                    String[] varNamePossibleTuple = Regex.Split(varName, @"(?=[[])");
                    if (cp.VariableValues.Where(v => v.VarName == varNamePossibleTuple[0]).Any() && result.Where(v => v.varName == varName).Count() == 0)
                    {
                        varNamePossibleTuple[1] = Regex.Replace(varNamePossibleTuple[1], @"(\$)|(\[)|(\])|( )", "");
                        String[] varNamePossibleMultiInstance = Regex.Split(varNamePossibleTuple[1], @"(?=[,])");

                        string value;

                        if (substr.Count(f => f == ',') > 0)
                        {
                            varNamePossibleMultiInstance[1] = Regex.Replace(varNamePossibleMultiInstance[1], @"(,)", "");

                            value = setForValidation.Where(cpi => cpi.varName == varNamePossibleTuple[0] && cpi.ocurrenceNumber != null && cpi.ocurrenceNumber.ToString() == varNamePossibleMultiInstance[1]).First().varValue;
                            ocurrenceNumber = varNamePossibleMultiInstance[1];
                        }
                        else
                        {
                            value = setForValidation.Where(cpi => cpi.varName == varNamePossibleTuple[0]).First().varValue;
                        }
                        if (value.Contains("["))
                        {
                            string[] values = value.TrimStart('[').TrimEnd(']').Split(',');
                            value = values[int.Parse(varNamePossibleMultiInstance[0])];
                        }
                        else
                        {
                            string[] values = value.TrimStart('{').TrimEnd('}').Split(',');
                            value = values[int.Parse(varNamePossibleTuple[0])];
                        }
                        varName = Regex.Replace(varName, @"(,\d*)", "");
                        result.Add((varName, value, ocurrenceNumber));
                    }
                }
            }
            return result;
        }
        private static string GetTestCodeFilenameNew(string testCode, string nameFile, VmLanguage language)
        {
            if (language.Name == "Java")
            {
                string find = "public class";
                int index = testCode.IndexOf(find);
                string name1 = testCode.Substring(index + find.Length + 1);
                int index1 = name1.IndexOf(" ");
                string name = name1.Substring(0, index1);
                return $@"{name}.java";
            }
            else
            {
                string testCodeExtension = language.SourceExtension ?? "txt";
                return $@"{nameFile}.{testCodeExtension}";
            }
        }
        private static HashSet<VmCodingProblemInstance> CreateCodingProblemInstanceNew(ICollection<VmVariableValue> codingProblemVariables, List<string> singleInstanceWithoutMemory, VmLoadAllDataInfo allDataInfo)
        {
            Dictionary<string, String> codingProblemInstanceVariables = GetCodingProblemVariablesNew(codingProblemVariables);
            HashSet<VmCodingProblemInstance> solution = new HashSet<VmCodingProblemInstance>();
            var codingProblemData = allDataInfo.CodingProblem;
            var studentInfo = allDataInfo.StudentInfo;
            foreach (var v in codingProblemInstanceVariables)
            {
                VmVariableValue varValue = codingProblemData.VariableValues.Where(vv => vv.VarName == v.Key).FirstOrDefault();
                VmCodingProblemInstance inst = new VmCodingProblemInstance
                {
                    Student = studentInfo,
                    CodingProblem = codingProblemData,
                    VarName = v.Key,
                    VarValue = v.Value,
                    idVariableValue = varValue.idVariableValue

                };
                if (!singleInstanceWithoutMemory.Any(varName => varName.Equals(v.Key)))
                {
                    //---------------------------------Add in CodingProblemInstance----------------------
                    string sqlQueryInsertCPI = $@"INSERT INTO [dbo].[CodingProblemInstance]
                                               ([CodingProblemId]
                                               ,[StudentId]
                                               ,[VarName]
                                               ,[VarValue]
                                               ,[idVariableValue])
                                         VALUES
                                               {codingProblemData.Id}
                                               ,{studentInfo.StudentId}
                                               ,{v.Key}
                                               ,{v.Value}
                                               ,{varValue.idVariableValue}";

                    var success = SQLHelper.RunSqlUpdate(sqlQueryInsertCPI);
                    //--------------------------------------------------------------------------------------------
                }
                solution.Add(inst);
            }
            return solution;

        }
        private static HashSet<VmCodingProblemInstance> CreateCodingProblemMultiInstanceNew(HashSet<(string varName, string possibleValues, int numberOfInstances)> missingInstances, (string, string[])[] multipleInstances, HashSet<(string varName, string occurenceNumber)> multipleInstanceWithoutMemory, VmLoadAllDataInfo allDataInfo)
        {
            Dictionary<string, string[]> sortedMultipleInstances = GetCodingProblemVariablesMultiInstance(missingInstances);
            HashSet<VmCodingProblemInstance> solution = new HashSet<VmCodingProblemInstance>();
            var codingProblemData = allDataInfo.CodingProblem;
            var studentInfo = allDataInfo.StudentInfo;
            foreach (var (varName, instances) in multipleInstances)
            {
                if (sortedMultipleInstances.Any(x => x.Key.Equals(varName)))
                {
                    var sortedValues = sortedMultipleInstances.First(x => x.Key.Equals(varName)).Value;
                    for (int i = 0; i < instances.Length; i++)
                    {
                        VmVariableValue varValue = codingProblemData.VariableValues.Where(vv => vv.VarName == varName).FirstOrDefault();
                        VmCodingProblemInstance inst = new VmCodingProblemInstance
                        {
                            Student = studentInfo,
                            CodingProblem = codingProblemData,
                            VarName = varName,
                            VarValue = sortedValues[i],
                            idVariableValue = varValue.idVariableValue,
                            occurrenceNumber = int.Parse(instances[i])

                        };
                        //TODO Check for instances with memory instead of without memory
                        if (!multipleInstanceWithoutMemory.Any(x => x.varName.Equals(varName) && x.occurenceNumber == instances[i]))
                        {
                            //---------------------------------Add in CodingProblemInstance----------------------
                            string sqlQueryInsertCPI = $@"INSERT INTO [dbo].[CodingProblemInstance]
                                               ([CodingProblemId]
                                               ,[StudentId]
                                               ,[VarName]
                                               ,[VarValue]
                                               ,[idVariableValue]
                                               ,[occurrenceNumber])
                                         VALUES
                                               {codingProblemData.Id}
                                               ,{studentInfo.StudentId}
                                               ,{varName}
                                               ,{sortedValues[i]}
                                               ,{varValue.idVariableValue}
                                               ,{int.Parse(instances[i])}";

                            var success = SQLHelper.RunSqlUpdate(sqlQueryInsertCPI);
                            //--------------------------------------------------------------------------------------------
                            //data.CodingProblemInstances.Add(inst);
                        }
                        solution.Add(inst);
                    }
                }
            }
            return solution;
        }
        private static Dictionary<string, String> GetCodingProblemVariablesNew(ICollection<VmVariableValue> variables)
        {
            var solution = new Dictionary<string, String>();
            int first, last, step, maxValue, aux;
            string[] separatingStrings = { ".." };
            Random rd = new Random();

            foreach (var var in variables)
            {
                //Split by "," but not by "/,"
                string[] separated = Regex.Split(var.possibleValues, @"(?<!\/)\,(?![^[]*]|[^{]*})");
                maxValue = 0;
                var weightedArray = new (String, int)[separated.Length];
                for (int i = 0; i < separated.Length; i++)
                {
                    //Asume that the var doesnt contain a ".." unless it is a number
                    string[] words = separated[i].Split(separatingStrings, System.StringSplitOptions.RemoveEmptyEntries);
                    if (words.Length >= 2)
                    {
                        first = Int32.Parse(words[0]);
                        words = words[1].Split(':');
                        if (words.Length >= 2)
                        {
                            last = Int32.Parse(words[0]);
                            step = Int32.Parse(words[1]);
                            int newStep = Math.Abs(last - first) / step;
                            aux = newStep + 1;
                            step = rd.Next(0, aux) * step;
                            maxValue += aux;
                            weightedArray[i] = ((first + step).ToString(), aux);
                        }
                        else
                        {
                            last = Int32.Parse(words[0]);
                            aux = Math.Abs(last - first) + 1;
                            maxValue += aux;
                            weightedArray[i] = ((rd.Next(first, last + 1)).ToString(), aux);

                        }
                    }
                    else if (words.Length > 0)
                    {
                        //If it's a string, replace any instance of "/," by ","
                        words[0] = words[0].Replace("/,", ",");
                        weightedArray[i] = (words[0], 1);
                        maxValue++;
                    }
                }
                int rand_no = rd.Next(0, maxValue);
                foreach (var element in weightedArray)
                {
                    if (rand_no < element.Item2)
                    {
                        solution.Add(var.VarName, element.Item1);
                        break;
                    }
                    rand_no = rand_no - element.Item2;
                }
            }
            return solution;
        }

        private static List<VmCodingProblemInstance> GetCodingProblemInstance(int codingProblemId)
        {
            string sqlQueryCodingPI = $@"select idCodingProblemInstance, CodingProblemId, StudentId, VarName, VarValue, idVariableValue, occurrenceNumber 
                                        from CodingProblemInstance
                                        where CodingProblemId = {codingProblemId}";

            var codingPI = SQLHelper.RunSqlQuery(sqlQueryCodingPI);
            List<VmCodingProblemInstance> codingProblemInstance = new List<VmCodingProblemInstance>();
            if (codingPI.Count > 0)
            {
                foreach (var i in codingPI)
                {
                    VmCodingProblemInstance value = new VmCodingProblemInstance() { idCodingProblemInstance = (int)i[0], CodingProblemId = (int)i[1], StudentId = (int)i[2], VarName = i[3].ToString(), VarValue = i[4].ToString(), idVariableValue = (int)i[5], occurrenceNumber = i[6].ToString() == "" ? (int?)null : (int)i[6] };
                    codingProblemInstance.Add(value);
                }
            }
            return codingProblemInstance;
        }
        private static VmLoadAllDataInfo GetCodingProblemAndStudentData(int codingProblemId, string hash)
        {
            string sqlQueryCodingProblem = $@"select Instructions, Script, Solution, ClassName, MethodName, ParameterTypes, 
								            Language, TestCaseClass, Before, After, MaxGrade, Title, Type, Attempts, Active, 
								            Role, Id, ExpectedOutput, Parameters, TestCode, TestCodeForStudent, st.StudentId, st.Test, st.Email
								            From Student st
											, CodingProblem cp
											where cp.Id = {codingProblemId} and st.Hash='{hash}'";

            var codingProblemData = SQLHelper.RunSqlQuery(sqlQueryCodingProblem);
            VmCodingProblem codingProblem = new VmCodingProblem();
            VmStudent studentInfo = new VmStudent();
            if (codingProblemData.Count > 0)
            {
                //----------------------------------VariableValue-----------------------------------
                string sqlQueryVariableValue = $@"select IdVariableValue, CodingProblemId, VarName, possibleValues 
                                            from VariableValue
                                            where CodingProblemId = {codingProblemId}";

                var variableValueData = SQLHelper.RunSqlQuery(sqlQueryVariableValue);
                List<VmVariableValue> variableValues = new List<VmVariableValue>();
                if (variableValueData.Count > 0)
                {
                    foreach (var i in variableValueData)
                    {
                        VmVariableValue value = new VmVariableValue() { idVariableValue = (int)i[0], CodingProblemId = (int)i[1], VarName = i[2].ToString(), possibleValues = i[3].ToString() };
                        variableValues.Add(value);
                    }
                }
                //------------------------------------------------------------------------
                var cp = codingProblemData.First();
                codingProblem = new VmCodingProblem
                {
                    Instructions = cp[0].ToString(),
                    Script = cp[1].ToString(),
                    Solution = cp[2].ToString(),
                    ClassName = cp[3].ToString(),
                    MethodName = cp[4].ToString(),
                    ParameterTypes = cp[5].ToString(),
                    Language = cp[6].ToString(),
                    TestCaseClass = cp[7].ToString(),
                    Before = cp[8].ToString(),
                    After = cp[9].ToString(),
                    MaxGrade = (int)cp[10],
                    Title = cp[11].ToString(),
                    Type = cp[12].ToString(),
                    Attempts = (int)cp[13],
                    Active = (bool)cp[14],
                    Role = (int)cp[15],
                    Id = (int)cp[16],
                    ExpectedOutput = cp[17].ToString(),
                    Parameters = cp[18].ToString(),
                    TestCode = cp[19].ToString(),
                    TestCodeForStudent = cp[20].ToString(),
                    VariableValues = variableValues
                };
                //--------------Student--------------
                studentInfo = new VmStudent
                {
                    StudentId = (int)cp[21],
                    Email = cp[23].ToString(),
                    Test = cp[22].ToString() == "" ? (bool?)null : (bool)cp[22]
                };
            }
            var model = new VmLoadAllDataInfo() { CodingProblem =codingProblem, StudentInfo = studentInfo };
            return model;
        }

        private static VmStudent GetStudentInfo(string hash)
        {
            string sqlQueryStudent = $@"select StudentId, UserName, Password, Name, Email, Test 
                                                from Student where Hash = '{hash}'";

            var studentData = SQLHelper.RunSqlQuery(sqlQueryStudent);
            VmStudent studentinfo = new VmStudent();
            if (studentData.Count > 0)
            {
                var st = studentData.First();
                studentinfo = new VmStudent
                {
                    StudentId = (int)st[0],
                    UserName = st[1].ToString(),
                    Password = st[2].ToString(),
                    Name = st[3].ToString(),
                    Email = st[4].ToString(),
                    Test = st[5].ToString() == "" ? (bool?)null : (bool)st[5]
                };
            }
            return studentinfo;
        }
        private static string GetStudentInfoId(string studentEmail)
        {
            string studentId = studentEmail;
            if (studentId.Contains('@'))
            {
                studentId = studentEmail.Split('@')[0];
            }
            studentId = studentId.Replace("-", "");
            studentId = studentId.Replace(".", "");
            return studentId;
        }
        private static VmLanguage GetLanguageInfo(string name)
        {
            string sqlQueryLanguage = $@"select Id, Name, Keywords, KeywordsOutput, SourceExtension
                                        from Language where Name = '{name}'";

            var languageData = SQLHelper.RunSqlQuery(sqlQueryLanguage);
            VmLanguage language = new VmLanguage();
            if (languageData.Count > 0)
            {
                var lng = languageData.First();
                language = new VmLanguage
                {
                    Id = (int)lng[0],
                    Name = lng[1].ToString(),
                    Keywords = lng[2].ToString(),
                    KeywordsOutput = lng[3].ToString(),
                    SourceExtension = lng[4].ToString()
                };
            }
            return language;
        }
        //=====================================================================================================================
    } // end of public static partial class Compiler

}

