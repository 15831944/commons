using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace System.Helper
{
    public class HtmlFilter
    {
        protected static readonly RegexOptions REGEX_FLAGS_SI = RegexOptions.IgnoreCase | RegexOptions.Singleline | RegexOptions.Compiled;

        private static string P_COMMENTS = "<!--(.*?)-->";

        private static Regex P_COMMENT = new Regex("^!--(.*)--$", REGEX_FLAGS_SI);

        private static string P_TAGS = "<(.*?)>";

        private static Regex P_END_TAG = new Regex("^/([a-z0-9]+)", REGEX_FLAGS_SI);

        private static Regex P_START_TAG = new Regex("^([a-z0-9]+)(.*?)(/?)$", REGEX_FLAGS_SI);

        private static Regex P_QUOTED_ATTRIBUTES = new Regex("([a-z0-9|(a-z0-9\\-a-z0-9)]+)=([\"'])(.*?)\\2", REGEX_FLAGS_SI);

        private static Regex P_UNQUOTED_ATTRIBUTES = new Regex("([a-z0-9]+)(=)([^\"\\s']+)", REGEX_FLAGS_SI);

        private static Regex P_PROTOCOL = new Regex("^([^:]+):", REGEX_FLAGS_SI);

        private static Regex P_ENTITY = new Regex("&#(\\d+);?");

        private static Regex P_ENTITY_UNICODE = new Regex("&#x([0-9a-f]+);?");

        private static Regex P_ENCODE = new Regex("%([0-9a-f]{2});?");

        private static Regex P_VALID_ENTITIES = new Regex("&([^&;]*)(?=(;|&|$))");

        private static Regex P_VALID_QUOTES = new Regex("(>|^)([^<]+?)(<|$)", RegexOptions.Singleline | RegexOptions.Compiled);

        private static string P_END_ARROW = "^>";

        private static string P_BODY_TO_END = "<([^>]*?)(?=<|$)";

        private static string P_XML_CONTENT = "(^|>)([^<]*?)(?=>)";

        private static string P_STRAY_LEFT_ARROW = "<([^>]*?)(?=<|$)";

        private static string P_STRAY_RIGHT_ARROW = "(^|>)([^<]*?)(?=>)";

        //private static string P_AMP = "&";
        private static string P_QUOTE = "\"";

        private static string P_LEFT_ARROW = "<";

        private static string P_RIGHT_ARROW = ">";

        private static string P_BOTH_ARROWS = "<>";

        // @xxx could grow large... maybe use sesat's ReferenceMap
        private static Dictionary<String, string> P_REMOVE_PAIR_BLANKS = new Dictionary<String, string>();

        private static Dictionary<String, string> P_REMOVE_SELF_BLANKS = new Dictionary<String, string>();
        /**
         * flag determining whether to try to make tags when presented with "unbalanced"
         * angle brackets (e.g. "<b text </b>" becomes "<b> text </b>").  If set to false,
         * unbalanced angle brackets will be html escaped.
         */

        protected static bool alwaysMakeTags = true;

        /**
         * flag determing whether comments are allowed in input String.
         */

        protected static bool stripComment = true;

        /// <summary>
        /// 不允许
        /// </summary>
        private String[] vDisallowed { get; set; }

        /// <summary>
        /// 允许
        /// </summary>
        protected Dictionary<String, List<String>> vAllowed { get; set; }

        /** counts of open tags for each (allowable) html element **/

        protected Dictionary<String, int> vTagCounts;

        /** html elements which must always be self-closing (e.g. "<img />") **/

        protected String[] vSelfClosingTags;

        /** html elements which must always have separate opening and closing tags (e.g. "<b></b>") **/

        protected String[] vNeedClosingTags;

        /** attributes which should be checked for valid protocols **/

        protected String[] vProtocolAtts;

        /** allowed protocols **/

        protected String[] vAllowedProtocols;

        /** tags which should be removed if they contain no content (e.g. "<b></b>" or "<b />") **/

        protected String[] vRemoveBlanks;

        /** entities allowed within html markup **/

        protected String[] vAllowedEntities;

        /// <summary>
        /// 是否为调试
        /// </summary>
        protected bool vDebug;

        public HtmlFilter() : this(false)
        {
        }

        public HtmlFilter(bool debug)
        {
            //List<Item> vAllowed = new List<Item>();
            this.vAllowed = new Dictionary<String, List<String>>();

            #region 允许通过数组

            this.vAllowed.Add("a", new List<string>() { "target", "href", "title", "class", "style" });
            this.vAllowed.Add("addr", new List<string>() { "title", "class", "style" });
            this.vAllowed.Add("address", new List<string>() { "class", "style" });
            this.vAllowed.Add("area", new List<string>() { "shape", "coords", "href", "alt" });
            this.vAllowed.Add("article", new List<string>() { });
            this.vAllowed.Add("aside", new List<string>() { });
            this.vAllowed.Add("audio", new List<string>() { "autoplay", "controls", "loop", "preload", "src", "class", "style" });
            this.vAllowed.Add("b", new List<string>() { "class", "style" });
            this.vAllowed.Add("bdi", new List<string>() { "dir" });
            this.vAllowed.Add("bdo", new List<string>() { "dir" });
            this.vAllowed.Add("big", new List<string>() { });
            this.vAllowed.Add("blockquote", new List<string>() { "cite", "class", "style" });
            this.vAllowed.Add("br", new List<string>() { });
            this.vAllowed.Add("caption", new List<string>() { "class", "style" });
            this.vAllowed.Add("center", new List<string>() { });
            this.vAllowed.Add("cite", new List<string>() { });
            this.vAllowed.Add("code", new List<string>() { "class", "style" });
            this.vAllowed.Add("col", new List<string>() { "align", "valign", "span", "width", "class", "style" });
            this.vAllowed.Add("colgroup", new List<string>() { "align", "valign", "span", "width", "class", "style" });
            this.vAllowed.Add("dd", new List<string>() { "class", "style" });
            this.vAllowed.Add("del", new List<string>() { "datetime" });
            this.vAllowed.Add("details", new List<string>() { "open" });
            this.vAllowed.Add("div", new List<string>() { "class", "style" });
            this.vAllowed.Add("dl", new List<string>() { "class", "style" });
            this.vAllowed.Add("dt", new List<string>() { "class", "style" });
            this.vAllowed.Add("em", new List<string>() { "class", "style" });
            this.vAllowed.Add("font", new List<string>() { "color", "size", "face" });
            this.vAllowed.Add("footer", new List<string>() { });
            this.vAllowed.Add("h1", new List<string>() { "class", "style" });
            this.vAllowed.Add("h2", new List<string>() { "class", "style" });
            this.vAllowed.Add("h3", new List<string>() { "class", "style" });
            this.vAllowed.Add("h4", new List<string>() { "class", "style" });
            this.vAllowed.Add("h5", new List<string>() { "class", "style" });
            this.vAllowed.Add("h6", new List<string>() { "class", "style" });
            this.vAllowed.Add("header", new List<string>() { });
            this.vAllowed.Add("hr", new List<string>() { });
            this.vAllowed.Add("i", new List<string>() { "class", "style" });
            this.vAllowed.Add("img", new List<string>() { "src", "alt", "title", "style", "width", "height", "id", "_src", "loadingclass", "class", "data-latex", "data-id", "data-type", "data-s" });
            this.vAllowed.Add("ins", new List<string>() { "datetime" });
            this.vAllowed.Add("li", new List<string>() { "class", "style" });
            this.vAllowed.Add("mark", new List<string>() { });
            this.vAllowed.Add("nav", new List<string>() { });
            this.vAllowed.Add("ol", new List<string>() { "class", "style" });
            this.vAllowed.Add("p", new List<string>() { "class", "style" });
            this.vAllowed.Add("pre", new List<string>() { "class", "style" });
            this.vAllowed.Add("s", new List<string>() { });
            this.vAllowed.Add("section", new List<string>() { });
            this.vAllowed.Add("small", new List<string>() { });
            this.vAllowed.Add("span", new List<string>() { "class", "style" });
            this.vAllowed.Add("sub", new List<string>() { "class", "style" });
            this.vAllowed.Add("sup", new List<string>() { "class", "style" });
            this.vAllowed.Add("strong", new List<string>() { "class", "style" });
            this.vAllowed.Add("table", new List<string>() { "width", "border", "align", "valign", "class", "style" });
            this.vAllowed.Add("tbody", new List<string>() { "align", "valign", "class", "style" });
            this.vAllowed.Add("td", new List<string>() { "width", "rowspan", "colspan", "align", "valign", "class", "style" });
            this.vAllowed.Add("tfoot", new List<string>() { "align", "valign", "class", "style" });
            this.vAllowed.Add("th", new List<string>() { "width", "rowspan", "colspan", "align", "valign", "class", "style" });
            this.vAllowed.Add("thead", new List<string>() { "align", "valign", "class", "style" });
            this.vAllowed.Add("tr", new List<string>() { "rowspan", "align", "valign", "class", "style" });
            this.vAllowed.Add("tt", new List<string>() { });
            this.vAllowed.Add("u", new List<string>() { });
            this.vAllowed.Add("ul", new List<string>() { "class", "style" });
            this.vAllowed.Add("video", new List<string>() { "autoplay", "controls", "loop", "preload", "src", "height", "width", "class", "style" });

            #endregion 允许通过数组

            this.vDebug = debug;
            this.vTagCounts = new Dictionary<String, int>();

            this.vSelfClosingTags = new String[] { "img" };
            this.vNeedClosingTags = new String[] { "a", "b", "strong", "i", "em" };
            this.vDisallowed = new String[] { "script" };
            this.vAllowedProtocols = new String[] { "http", "mailto", "data" }; // no ftp，data是base64图片的开头
            this.vProtocolAtts = new String[] { "src", "href" };
            this.vRemoveBlanks = new String[] { "a", "b", "strong", "i", "em" };
            this.vAllowedEntities = new String[] { "amp", "gt", "lt", "quot" };
            stripComment = true;
            alwaysMakeTags = true;
        }

        protected void reset()
        {
            this.vTagCounts = new Dictionary<String, int>();
        }

        protected void debug(String msg)
        {
            if (this.vDebug)
            {
                System.Diagnostics.Debug.WriteLine(msg);
            }
        }

        //---------------------------------------------------------------
        // my versions of some PHP library functions

        public static String chr(int dec)
        {
            return "" + ((char)dec);
        }

        /// <summary>
        /// 转换成实体字符
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static String htmlSpecialChars(String str)
        {
            str = str.Replace(P_QUOTE, "&quot;");
            str = str.Replace(P_LEFT_ARROW, "&lt;");
            str = str.Replace(P_RIGHT_ARROW, "&gt;");
            str = str.Replace("\n", "<br>");
            return str;
        }

        //---------------------------------------------------------------

        /**
         * given a user submitted input String, filter out any invalid or restricted
         * html.
         *
         * @param input text (i.e. submitted by a user) than may contain html
         * @return "clean" version of input, with only valid, whitelisted html elements allowed
         */

        public String filter(String input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return string.Empty;
            }

            this.reset();
            String s = input;

            this.debug("************************************************");
            this.debug("              INPUT: " + input);

            s = this.escapeComments(s);
            this.debug("     escapeComments: " + s);

            s = this.balanceHTML(s);
            this.debug("        balanceHTML: " + s);

            s = this.checkTags(s);
            this.debug("          checkTags: " + s);

            s = this.processRemoveBlanks(s);
            this.debug("processRemoveBlanks: " + s);

            s = this.validateEntities(s);
            this.debug("    validateEntites: " + s);

            this.debug("************************************************\n\n");

            var sql = "and|exec|insert|select|delete|update|chr|mid|master|or|truncate|char|declare|join|exec|xp_|sp_|execute";
            foreach (var w in sql.Split('|'))
            {
                s = Regex.Replace(s, w, "", RegexOptions.IgnoreCase);
            }
            return s;
        }

        protected String escapeComments(String s)
        {
            return Regex.Replace(s, P_COMMENTS, new MatchEvaluator(ConverMatchComments), RegexOptions.Singleline);
        }

        protected String regexReplace(String regex_pattern, String replacement, String s)
        {
            return Regex.Replace(s, regex_pattern, replacement);
        }

        protected String balanceHTML(String s)
        {
            if (alwaysMakeTags)
            {
                //
                // try and form html
                //
                s = this.regexReplace(P_END_ARROW, "", s);
                s = this.regexReplace(P_BODY_TO_END, "<$1>", s);
                s = this.regexReplace(P_XML_CONTENT, "$1<$2", s);
            }
            else
            {
                //
                // escape stray brackets
                //
                s = this.regexReplace(P_STRAY_LEFT_ARROW, "&lt;$1", s);
                s = this.regexReplace(P_STRAY_RIGHT_ARROW, "$1$2&gt;<", s);

                //
                // the last regexp causes '<>' entities to appear
                // (we need to do a lookahead assertion so that the last bracket can
                // be used in the next pass of the regexp)
                //
                s = s.Replace(P_BOTH_ARROWS, "");
            }
            return s;
        }

        protected String checkTags(String s)
        {
            //替换不允许标签
            foreach (var item in this.vDisallowed)
            {
                s = Regex.Replace(s, string.Format(@"<{0}\b(.)*?>(.)+?</{0}>", item), "");
            }
            s = Regex.Replace(s, P_TAGS, new MatchEvaluator(this.ConverMatchTags), RegexOptions.Singleline);

            // these get tallied in processTag
            // (remember to reset before subsequent calls to filter method)
            foreach (String key in this.vTagCounts.Keys)
            {
                for (int ii = 0; ii < this.vTagCounts[key]; ii++)
                {
                    s += "</" + key + ">";
                }
            }

            return s;
        }

        protected String processRemoveBlanks(String s)
        {
            foreach (String tag in this.vRemoveBlanks)
            {
                s = this.regexReplace("<" + tag + "(\\s[^>]*)?></" + tag + ">", "", s);
                s = this.regexReplace("<" + tag + "(\\s[^>]*)?/>", "", s);
            }
            return s;
        }

        private String processTag(String s)
        {
            // ending tags
            Match m = P_END_TAG.Match(s);
            if (m.Success)
            {
                string name = m.Groups[1].Value.ToLower();
                if (this.allowed(name))
                {
                    if (!inArray(name, this.vSelfClosingTags))
                    {
                        if (this.vTagCounts.ContainsKey(name))
                        {
                            this.vTagCounts[name] = this.vTagCounts[name] - 1;
                            return "</" + name + ">";
                        }
                    }
                }
            }

            // starting tags
            m = P_START_TAG.Match(s);
            if (m.Success)
            {
                String name = m.Groups[1].Value.ToLower();
                String body = m.Groups[2].Value;
                String ending = m.Groups[3].Value;

                //debug( "in a starting tag, name='" + name + "'; body='" + body + "'; ending='" + ending + "'" );
                if (this.allowed(name))
                {
                    String params1 = "";

                    MatchCollection m2 = P_QUOTED_ATTRIBUTES.Matches(body);
                    MatchCollection m3 = P_UNQUOTED_ATTRIBUTES.Matches(body);
                    List<String> paramNames = new List<String>();
                    List<String> paramValues = new List<String>();
                    foreach (Match match in m2)
                    {
                        paramNames.Add(match.Groups[1].Value); //([a-z0-9]+)
                        paramValues.Add(match.Groups[3].Value); //(.*?)
                    }
                    foreach (Match match in m3)
                    {
                        paramNames.Add(match.Groups[1].Value); //([a-z0-9]+)
                        paramValues.Add(match.Groups[3].Value); //([^\"\\s']+)
                    }

                    String paramName, paramValue;
                    for (int ii = 0; ii < paramNames.Count; ii++)
                    {
                        paramName = paramNames[ii].ToLower();
                        paramValue = paramValues[ii];

                        if (this.allowedAttribute(name, paramName))
                        {
                            if (inArray(paramName, this.vProtocolAtts))
                            {
                                paramValue = this.processParamProtocol(paramValue);
                            }
                            params1 += " " + paramName + "=\"" + paramValue + "\"";
                        }
                    }

                    if (inArray(name, this.vSelfClosingTags))
                    {
                        ending = " /";
                    }

                    if (inArray(name, this.vNeedClosingTags))
                    {
                        ending = "";
                    }

                    if (ending == null || ending.Length < 1)
                    {
                        if (this.vTagCounts.ContainsKey(name))
                        {
                            this.vTagCounts[name] = this.vTagCounts[name] + 1;
                        }
                        else
                        {
                            this.vTagCounts.Add(name, 1);
                        }
                    }
                    else
                    {
                        ending = " /";
                    }
                    return "<" + name + params1 + ending + ">";
                }
                else
                {
                    return "";
                }
            }

            // comments
            m = P_COMMENT.Match(s);
            if (!stripComment && m.Success)
            {
                return "<" + m.Value + ">";
            }

            return "";
        }

        private String processParamProtocol(String s)
        {
            s = this.decodeEntities(s);
            Match m = P_PROTOCOL.Match(s);
            if (m.Success)
            {
                String protocol = m.Groups[1].Value;
                if (!inArray(protocol, this.vAllowedProtocols))
                {
                    // bad protocol, turn into local anchor link instead
                    s = "#" + s.Substring(protocol.Length + 1, s.Length - protocol.Length - 1);
                    if (s.StartsWith("#//"))
                    {
                        s = "#" + s.Substring(3, s.Length - 3);
                    }
                }
            }
            return s;
        }

        private String decodeEntities(String s)
        {
            s = P_ENTITY.Replace(s, new MatchEvaluator(this.ConverMatchEntity));

            s = P_ENTITY_UNICODE.Replace(s, new MatchEvaluator(this.ConverMatchEntityUnicode));

            s = P_ENCODE.Replace(s, new MatchEvaluator(this.ConverMatchEntityUnicode));

            s = this.validateEntities(s);
            return s;
        }

        private String validateEntities(String s)
        {
            s = P_VALID_ENTITIES.Replace(s, new MatchEvaluator(this.ConverMatchValidEntities));
            s = P_VALID_QUOTES.Replace(s, new MatchEvaluator(this.ConverMatchValidQuotes));
            return s;
        }

        private static bool inArray(String s, String[] array)
        {
            foreach (String item in array)
            {
                if (item != null && item.Equals(s))
                {
                    return true;
                }
            }
            return false;
        }

        private bool allowed(String name)
        {
            return (this.vAllowed.Count == 0 || this.vAllowed.ContainsKey(name)) && !inArray(name, this.vDisallowed);
        }

        private bool allowedAttribute(String name, String paramName)
        {
            return this.allowed(name) && (this.vAllowed.Count == 0 || this.vAllowed[name].Contains(paramName));
        }

        private String checkEntity(String preamble, String term)
        {
            return ";".Equals(term) && this.isValidEntity(preamble)
                    ? '&' + preamble
                    : "&amp;" + preamble;
        }

        private bool isValidEntity(String entity)
        {
            return inArray(entity, this.vAllowedEntities);
        }

        private static string ConverMatchComments(Match match)
        {
            string matchValue = "<!--" + htmlSpecialChars(match.Groups[1].Value) + "-->";
            return matchValue;
        }

        private string ConverMatchTags(Match match)
        {
            string matchValue = this.processTag(match.Groups[1].Value);
            return matchValue;
        }

        private string ConverMatchEntity(Match match)
        {
            string v = match.Groups[1].Value;
            int decimal1 = int.Parse(v);
            return chr(decimal1);
        }

        private string ConverMatchEntityUnicode(Match match)
        {
            string v = match.Groups[1].Value;
            int decimal1 = Convert.ToInt32("0x" + v, 16);
            return chr(decimal1);
        }

        private string ConverMatchValidEntities(Match match)
        {
            String one = match.Groups[1].Value; //([^&;]*)
            String two = match.Groups[2].Value; //(?=(;|&|$))
            return this.checkEntity(one, two);
        }

        private string ConverMatchValidQuotes(Match match)
        {
            String one = match.Groups[1].Value; //(>|^)
            String two = match.Groups[2].Value; //([^<]+?)
            String three = match.Groups[3].Value;//(<|$)
            return one + this.regexReplace(P_QUOTE, "&quot;", two) + three;
        }

        public bool isAlwaysMakeTags()
        {
            return alwaysMakeTags;
        }

        public bool isStripComments()
        {
            return stripComment;
        }

        private class Item
        {
            public string name { get; set; }

            public List<string> parameter { get; set; }
        }
    }
}