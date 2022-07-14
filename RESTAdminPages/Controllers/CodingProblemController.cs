using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using EFModel;

namespace RESTAdminPages.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class CodingProblemController : ApiController
    {
        private MaterialEntities db = new MaterialEntities();

        public class CodingProblemInput
        {
            public string Instructions { get; set; }
            public string Language { get; set; }
            public string Script { get; set; }
            public string Solution { get; set; }
            public string Before { get; set; }
            public string After { get; set; }
            public string ExpectedOutput { get; set; }
            public string TestCode { get; set; }
            public string TestCodeForStudent { get; set; }
            public string ParameterTypes { get; set; }
            public int MaxGrade { get; set; }
            public string Title { get; set; }
            public bool Active { get; set; }
            public string Type { get; set; }
            public int Attempts { get; set; }
            public int Role { get; set; }
            public Dictionary<string, string> VarValuePairs { get; set; }
        }
        // GET: api/CodingProblem/5
        public HttpResponseMessage Get(int Id)
        {
            try
            {
                CodingProblem cp = db.CodingProblems.Find(Id);
                if (cp == null)
                {
                    HttpResponseMessage err = Request.CreateResponse(HttpStatusCode.BadRequest, "Bad Id");
                    return err;
                }
                List<XElement> VariableValues = new List<XElement>();
                IEnumerable<VariableValue> setVariableValue = cp.VariableValues.OrderBy(vv => vv.possibleValues);

                foreach (VariableValue vv in setVariableValue)
                {
                    XElement variable = new XElement("VariableValueElement",
                        new XElement("Name", vv.VarName),
                        new XElement("IdVariableValue", vv.idVariableValue),
                        new XElement("CodingProblem", vv.CodingProblemId),
                        new XElement("PossibleValues", vv.possibleValues));
                    VariableValues.Add(variable);
                }

                XElement myObj = new XElement("CodingProblem",
                    new XElement("Instructions", cp.Instructions),
                    new XElement("Script", cp.Script),
                    new XElement("Solution", cp.Solution),
                    new XElement("ClassName", cp.ClassName),
                    new XElement("MethodName", cp.MethodName),
                    new XElement("ParameterTypes", cp.ParameterTypes),
                    new XElement("Language", cp.Language),
                    new XElement("TestCaseClass", cp.TestCaseClass),
                    new XElement("Before", cp.Before),
                    new XElement("After", cp.After),
                    new XElement("MaxGrade", cp.MaxGrade),
                    new XElement("Title", cp.Title),
                    new XElement("Type", cp.Type),
                    new XElement("Active", cp.Active),
                    new XElement("Attempts", cp.Attempts),
                    new XElement("Role", cp.Role),
                    new XElement("Id", cp.Id),
                    new XElement("ExpectedOutput", cp.ExpectedOutput),
                    new XElement("Parameters", cp.Parameters),
                    new XElement("TestCode", cp.TestCode),
                    new XElement("TestCodeForStudent", cp.TestCodeForStudent),
                    new XElement("VariableValues", VariableValues));

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, myObj);
                return response;
            }
            catch (Exception e)
            {
                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.BadRequest, e.Message);
                return response;
            }
        }

        // GET: api/CodingProblem
        public HttpResponseMessage Get(string name=null, int idCourse=0, int idQuarter=0, int idInstance=0, int idModule=0)
        {
            try
            {
                List<XElement> result = new List<XElement>();
                if (idCourse * idQuarter * idInstance * idModule != 0)
                {
                    IQueryable<CourseInstanceCodingProblem> filteredCodingProblems = db.CourseInstanceCodingProblems;

                    if (name != null && name != "")
                        filteredCodingProblems = filteredCodingProblems.Where(x => x.CodingProblem.Title.Contains(name));
                    if (idCourse > 0)
                        filteredCodingProblems = filteredCodingProblems.Where(x => x.CourseInstance.CourseId == idCourse);
                    if (idQuarter > 0)
                        filteredCodingProblems = filteredCodingProblems.Where(x => x.CourseInstance.QuarterId == idQuarter);
                    if (idInstance > 0)
                        filteredCodingProblems = filteredCodingProblems.Where(x => x.CourseInstanceId == idInstance);
                    if (idModule > 0)
                        filteredCodingProblems = filteredCodingProblems.Where(x => x.ModuleObjectiveId == idModule);
                    var codingProblems = filteredCodingProblems.Select(x => new { Id = x.CodingProblem.Id, Title = x.CodingProblem.Title }).Distinct().ToList();
                    foreach (var cp in codingProblems)
                    {
                        XElement codingProblem = new XElement("CodingProblem",
                            new XElement("Id", cp.Id),
                            new XElement("Title", cp.Title)
                        );
                        result.Add(codingProblem);
                    }
                }
                else
                {
                    IQueryable<CodingProblem> allCodingProblems = db.CodingProblems;
                    if (name != null && name != "")
                        allCodingProblems = allCodingProblems.Where(x => x.Title.Contains(name));
                    var codingProblems = allCodingProblems.Select(x => new { Id = x.Id, Title = x.Title }).Distinct().ToList();
                    foreach (var cp in codingProblems)
                    {
                        XElement codingProblem = new XElement("CodingProblem",
                            new XElement("Id", cp.Id),
                            new XElement("Title", cp.Title)
                        );
                        result.Add(codingProblem);
                    }
                }
                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, result);
                return response;
            }
            catch (Exception e)
            {
                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.BadRequest, e.Message);
                return response;
            }
        }

        // POST: api/CodingProblem
        public HttpResponseMessage Post([FromBody] CodingProblemInput codingProblem)
        {
            (bool, string) validateCP = CodingProblemValidation(codingProblem);
            if (validateCP.Item1)
            {
                List<string> errors = new List<string>();
                try
                {
                    CodingProblem cp = new CodingProblem();
                    cp.Instructions = codingProblem.Instructions;
                    cp.Script = codingProblem.Script;
                    cp.Solution = codingProblem.Solution;
                    cp.Language = codingProblem.Language;
                    cp.Before = codingProblem.Before;
                    cp.After = codingProblem.After;
                    cp.MaxGrade = codingProblem.MaxGrade;
                    cp.Title = codingProblem.Title;
                    cp.Active = codingProblem.Active;
                    cp.Role = codingProblem.Role;
                    cp.ExpectedOutput = codingProblem.ExpectedOutput;
                    cp.TestCode = codingProblem.TestCode;
                    cp.Type = codingProblem.Type;
                    cp.TestCodeForStudent = codingProblem.TestCodeForStudent;
                    cp.Attempts = codingProblem.Attempts;
                    cp.ClassName = "";
                    cp.MethodName = "";
                    cp.ParameterTypes = "";
                    cp.Parameters = "";
                    cp.TestCaseClass = "";
                    cp = db.CodingProblems.Add(cp);

                    foreach (var i in codingProblem.VarValuePairs)
                    {
                        VariableValue varValues = new VariableValue();
                        varValues.VarName = i.Key.Trim();
                        varValues.possibleValues = i.Value.Trim();
                        varValues.CodingProblem = cp;
                        (bool, string) validation = variableValidation(varValues);

                        if (validation.Item1)
                        {
                            db.VariableValues.Add(varValues);
                        }
                        else
                        {
                            errors.Add(validation.Item2);
                        }
                    }
                }
                catch (Exception e)
                {
                    HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.BadRequest, e.Message);
                    return response;
                }
                if (errors.Count == 0)
                {
                    db.SaveChanges();

                    HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, "Saved Successfully");
                    return response;
                }
                else
                {
                    string errorString = errors.Aggregate((i, j) => i + ", " + j).ToString();
                    HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.BadRequest, errorString);
                    return response;
                }
            }
            else
            {
                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.BadRequest, validateCP.Item2);
                return response;
            }
        }

        
        // PUT: api/CodingProblem/5
        public HttpResponseMessage Put(int id, [FromBody] CodingProblemInput codingProblemUpd)
        {
            (bool, string) validateCP = CodingProblemValidation(codingProblemUpd);
            if (validateCP.Item1)
            {
                List<string> errors = new List<string>();
                CodingProblem codingproblem = db.CodingProblems.Find(id);
                if (codingproblem == null)
                {
                    HttpResponseMessage err = Request.CreateResponse(HttpStatusCode.BadRequest, "Bad Id");
                    return err;
                }
                else
                {
                    try
                    {
                        codingproblem.Instructions = codingProblemUpd.Instructions;
                        codingproblem.Script = codingProblemUpd.Script;
                        codingproblem.Solution = codingProblemUpd.Solution;
                        codingproblem.Language = codingProblemUpd.Language;
                        codingproblem.Before = codingProblemUpd.Before;
                        codingproblem.After = codingProblemUpd.After;
                        codingproblem.MaxGrade = codingProblemUpd.MaxGrade;
                        codingproblem.Title = codingProblemUpd.Title;
                        codingproblem.Active = codingProblemUpd.Active;
                        codingproblem.Role = codingProblemUpd.Role;
                        codingproblem.ExpectedOutput = codingProblemUpd.ExpectedOutput;
                        codingproblem.TestCode = codingProblemUpd.TestCode;
                        codingproblem.TestCodeForStudent = codingProblemUpd.TestCodeForStudent;
                        codingproblem.Attempts = codingProblemUpd.Attempts;

                        foreach (var i in codingProblemUpd.VarValuePairs)
                        {
                            VariableValue varValues = new VariableValue();
                            varValues.VarName = i.Key.Trim();
                            varValues.possibleValues = i.Value.Trim();
                            varValues.CodingProblem = codingproblem;
                            (bool, string) validation = variableValidation(varValues);
                            if (validation.Item1)
                            {
                                IQueryable<VariableValue> variableValue = db.VariableValues.Where(vv => vv.CodingProblemId == codingproblem.Id && vv.VarName == i.Key);
                                if (!variableValue.Any())
                                {
                                    db.VariableValues.Add(varValues);
                                }
                                else if (variableValue.First().possibleValues != i.Value)
                                {
                                    db.VariableValues.Remove(variableValue.First());
                                    db.VariableValues.Add(varValues);
                                }
                                IQueryable<VariableValue> variableValueList = db.VariableValues.Where(vv => vv.CodingProblemId == codingproblem.Id);
                                foreach (VariableValue vv in variableValueList)
                                {
                                    if (!codingProblemUpd.VarValuePairs.ContainsKey(i.Key))
                                    {
                                        db.VariableValues.Remove(vv);
                                    }
                                }
                            }
                            else
                            {
                                errors.Add(validation.Item2);
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.BadRequest, e.Message);
                        return response;
                    }
                }
                if (errors.Count == 0)
                {
                    db.SaveChanges();
                    HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, "Updated Successfully");
                    return response;
                }
                else
                {
                    string errorString = errors.Aggregate((i, j) => i + ", " + j).ToString();
                    HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.BadRequest, errorString);
                    return response;
                }
            }
            else
            {
                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.BadRequest, validateCP.Item2);
                return response;
            }
        }

        // DELETE: api/CodingProblem/5
        public void Delete(int id)
        {
        }

        protected (bool, string) CodingProblemValidation(CodingProblemInput codingProblem)
        {
            bool result = true;
            string fieldName = "Sorry! Operation has been failed. Required Fields: ";

            if (string.IsNullOrWhiteSpace(codingProblem.Type))
            {
                fieldName += " Type -";
                result = false;
            }

            if (string.IsNullOrWhiteSpace(codingProblem.Language))
            {
                fieldName += " Language -";
                result = false;
            }

            if (string.IsNullOrWhiteSpace(codingProblem.MaxGrade.ToString()))
            {
                fieldName += " Max Grade -";
                result = false;
            }

            if (string.IsNullOrWhiteSpace(codingProblem.Title))
            {
                fieldName += " Title -";
                result = false;
            }

            if (string.IsNullOrWhiteSpace(codingProblem.Attempts.ToString()))
            {
                fieldName += " Attempts -";
                result = false;
            }

            if (string.IsNullOrWhiteSpace(codingProblem.Role.ToString()))
            {
                fieldName += " Role -";
                result = false;
            }

            if (!result)
            {
                return (result, fieldName);
            }
            else
            {
                return (result, "");
            }
        }

        protected (bool, string) variableValidation(VariableValue vv)
        {
            string msg = "";

            if (vv.VarName != "" && vv.possibleValues != "")
            {
                string pattern = @".*\$.*|.*\{.*|.*\}.*|^studentid$"; //Not allow variables with $,{,} and "studentid"
                bool nameNotValid = Regex.IsMatch(vv.VarName, pattern, RegexOptions.IgnoreCase);
                if (nameNotValid)
                {
                    msg = "Name " + vv.VarName + " is not valid";
                    return (false, msg);
                }
                else
                {
                    string[] possiblesValues = Regex.Split(vv.possibleValues, @"(?<!\/)\,(?![^[]*]|[^{]*})");
                    if (Regex.IsMatch(vv.possibleValues, "\\["))
                    {
                        foreach (String possiblesValuesItem in possiblesValues)
                        {
                            bool isArray = Regex.IsMatch(possiblesValuesItem, @"^\[([\w]+((,[\w]*)+|([\w]*)+)\]$)|^\{([\w]+((,[\w]*)+|([\w]*)+)\}$)");
                            if (!isArray)
                            {
                                msg = "The variable named " + vv.VarName + " has a wrongly defined " + possiblesValuesItem;
                                return (false, msg);
                            }
                        }
                        return (true, msg);
                    }
                    else
                    {
                        foreach (String possiblesValuesItem in possiblesValues)
                        {
                            pattern = "\\.\\.";
                            bool isRange = Regex.IsMatch(possiblesValuesItem, pattern);
                            if (isRange)
                            {
                                string[] range = possiblesValuesItem.Trim().Split(new[] { ".." }, StringSplitOptions.RemoveEmptyEntries);
                                if (range.Count() > 1)
                                {
                                    pattern = "\\:";
                                    bool haveColon = Regex.IsMatch(range[1], pattern);
                                    string[] ColInRange = range[1].Trim().Split(new[] { ':' }, StringSplitOptions.RemoveEmptyEntries);
                                    if (haveColon && ColInRange.Count() != 2)
                                    {
                                        msg = "The variable named " + vv.VarName.Trim() + " has a wrongly defined range";
                                        return (false, msg);
                                    }
                                }
                                else
                                {
                                    msg = "The variable named " + vv.VarName.Trim() + " has a wrongly defined range";
                                    return (false, msg);
                                }
                            }
                        }
                        return (true, msg);
                    }
                }
            }
            else if (vv.VarName == "" && vv.possibleValues == "")
            {
                msg = "Variable is empty";
                return (false, msg);
            }
            else if (vv.VarName != "" && vv.possibleValues == "")
            {
                msg = "Possible values is required";
                return (false, msg);
            }
            else if (vv.VarName == "" && vv.possibleValues != "")
            {
                msg = "Variable name is required";
                return (false, msg);
            }
            else
            {
                return (true, msg);
            }
        }
    }
}
