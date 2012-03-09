using System;
using System.IO;
using System.Reflection;
using System.Text;
using System.Web.Mvc;
using System.Web.UI;

namespace XmlRpcMvc
{
    public class XmlRpcServiceOverviewResult : ContentResult
    {
        private readonly bool _generateOverview;
        private readonly Type[] _services;

        public XmlRpcServiceOverviewResult(
            bool generateOverview, params Type[] services)
        {
            _generateOverview = generateOverview;
            _services = services;

            ContentType = "text/html";
            ContentEncoding = Encoding.UTF8;
        }

        public override void ExecuteResult(ControllerContext context)
        {
            if (!_generateOverview)
            {
                new HttpNotFoundResult().ExecuteResult(context);
                return;
            }

            var title = context.Controller.ValueProvider.GetValue("action").AttemptedValue;

            var methods = XmlRpcRequestParser.GetMethods(_services);

            using (var stringWriter = new StringWriter())
            using (var htmlWriter = new HtmlTextWriter(stringWriter))
            {
                htmlWriter.RenderBeginTag(HtmlTextWriterTag.Html);
                {
                    htmlWriter.RenderBeginTag(HtmlTextWriterTag.Head);
                    {
                        // Version Info
                        htmlWriter.Write("<!--");
                        htmlWriter.Write("XmlRpcMvc {0}", Assembly.GetExecutingAssembly().GetName().Version);
                        htmlWriter.Write("-->");

                        htmlWriter.RenderBeginTag(HtmlTextWriterTag.Title);
                        {
                            htmlWriter.Write(title);
                        }
                        htmlWriter.RenderEndTag();

                        // <meta name="robots" content="noindex" />
                        htmlWriter.AddAttribute(HtmlTextWriterAttribute.Name, "robots");
                        htmlWriter.AddAttribute(HtmlTextWriterAttribute.Content, "noindex");
                        htmlWriter.RenderBeginTag(HtmlTextWriterTag.Meta);
                        htmlWriter.RenderEndTag();

                        htmlWriter.AddAttribute(HtmlTextWriterAttribute.Type, "text/css");
                        htmlWriter.RenderBeginTag(HtmlTextWriterTag.Style);
                        {
                            htmlWriter.Write(@"
body {
    font-family: Segoe UI Light, Segoe WP Light, Segoe UI, Helvetica, sans-serif;
    padding: 0;
    margin: 0;
}

body > div {
    padding: 0 20px;
}

body > div > div {
    margin-bottom: 50px;
    border-top: 1px solid #CCCCCC;
    width: 90%;
}

h1 {
    background-color: #1BA1E2;
    color: white;
    padding: 5px 20px;
}

h2 {
    color: #1BA1E2;
}

ul {
    margin-bottom: 30px;
}

li {
    margin-bottom: 10px;
}

li > a {
    color: #000000;
}

table {
    width: 100%;
}

tr:nth-child(even) {
    background: #CCCCCC
}

tr:nth-child(odd) {
    background: #FFFFFF
}

td {
    height: 40px;
    vertical-align: middle;
    padding: 0 10px;
}
");
                        }
                        htmlWriter.RenderEndTag();
                    }
                    htmlWriter.RenderEndTag();

                    htmlWriter.RenderBeginTag(HtmlTextWriterTag.Body);
                    {
                        htmlWriter.RenderBeginTag(HtmlTextWriterTag.H1);
                        {
                            htmlWriter.Write(title);
                        }
                        htmlWriter.RenderEndTag();

                        htmlWriter.RenderBeginTag(HtmlTextWriterTag.Div);
                        {
                            htmlWriter.RenderBeginTag(HtmlTextWriterTag.P);
                            {
                                htmlWriter.Write("The following methods are supported:");
                            }
                            htmlWriter.RenderEndTag();

                            // Method Names
                            htmlWriter.RenderBeginTag(HtmlTextWriterTag.Ul);
                            {
                                foreach (var method in methods)
                                {
                                    // Method Name
                                    htmlWriter.RenderBeginTag(HtmlTextWriterTag.Li);
                                    {
                                        htmlWriter.AddAttribute(HtmlTextWriterAttribute.Href, string.Concat("#", method.Value.Name));
                                        htmlWriter.RenderBeginTag(HtmlTextWriterTag.A);
                                        {
                                            htmlWriter.Write(method.Value.Name);
                                        }
                                        htmlWriter.RenderEndTag();
                                    }
                                    htmlWriter.RenderEndTag();
                                }
                            }
                            htmlWriter.RenderEndTag();

                            foreach (var method in methods)
                            {
                                var mi = method.Value.MethodInfo;

                                htmlWriter.RenderBeginTag(HtmlTextWriterTag.Div);
                                {
                                    // Method name
                                    htmlWriter.RenderBeginTag(HtmlTextWriterTag.H2);
                                    {
                                        htmlWriter.AddAttribute(HtmlTextWriterAttribute.Name, method.Value.Name);
                                        htmlWriter.RenderBeginTag(HtmlTextWriterTag.A);
                                        {
                                            htmlWriter.Write(method.Value.Name);
                                        }
                                        htmlWriter.RenderEndTag();
                                    }
                                    htmlWriter.RenderEndTag();

                                    // "Parameters" headline
                                    htmlWriter.RenderBeginTag(HtmlTextWriterTag.H3);
                                    {
                                        htmlWriter.Write("Parameters");
                                    }
                                    htmlWriter.RenderEndTag();

                                    // "Parameters" table
                                    htmlWriter.RenderBeginTag(HtmlTextWriterTag.Table);
                                    {
                                        var parameters = mi.GetParameters();

                                        foreach (var parameter in parameters)
                                        {
                                            htmlWriter.RenderBeginTag(HtmlTextWriterTag.Tr);
                                            {
                                                htmlWriter.AddStyleAttribute(HtmlTextWriterStyle.Width, "30%");
                                                htmlWriter.RenderBeginTag(HtmlTextWriterTag.Td);
                                                {
                                                    htmlWriter.Write(parameter.ParameterType.Name);
                                                }
                                                htmlWriter.RenderEndTag();

                                                htmlWriter.RenderBeginTag(HtmlTextWriterTag.Td);
                                                {
                                                    htmlWriter.Write(parameter.Name);
                                                }
                                                htmlWriter.RenderEndTag();
                                            }
                                            htmlWriter.RenderEndTag();
                                        }
                                    }
                                    htmlWriter.RenderEndTag();

                                    // "Return Value" headline
                                    htmlWriter.RenderBeginTag(HtmlTextWriterTag.H3);
                                    {
                                        htmlWriter.Write("Return Value");
                                    }
                                    htmlWriter.RenderEndTag();

                                    // "Return Value" table
                                    htmlWriter.RenderBeginTag(HtmlTextWriterTag.Table);
                                    {
                                        htmlWriter.RenderBeginTag(HtmlTextWriterTag.Tr);
                                        {
                                            htmlWriter.AddStyleAttribute(HtmlTextWriterStyle.Width, "30%");
                                            htmlWriter.RenderBeginTag(HtmlTextWriterTag.Td);
                                            {
                                                htmlWriter.Write(mi.ReturnType.Name);
                                            }
                                            htmlWriter.RenderEndTag();

                                            htmlWriter.RenderBeginTag(HtmlTextWriterTag.Td);
                                            {
                                                htmlWriter.Write(
                                                    !string.IsNullOrEmpty(method.Value.Description)
                                                        ? method.Value.Description
                                                        : "-");
                                            }
                                            htmlWriter.RenderEndTag();
                                        }
                                        htmlWriter.RenderEndTag();
                                    }
                                    htmlWriter.RenderEndTag();
                                }
                                htmlWriter.RenderEndTag();
                            }
                        }
                        htmlWriter.RenderEndTag();
                    }
                    htmlWriter.RenderEndTag();
                }
                htmlWriter.RenderEndTag();

                Content = stringWriter.ToString();
                base.ExecuteResult(context);
            }
        }
    }
}
