using System.Collections;
using System.IO;
using System.Text.RegularExpressions;

namespace System
{
    /// <summary>
    /// �ִʸ�����
    /// </summary>
    public class SegList
    {
        public int MaxLength;

        private ArrayList m_seg;

        public int Count
        {
            get
            {
                return this.m_seg.Count;
            }
        }

        public SegList()
        {
            this.m_seg = new ArrayList();
            this.MaxLength = 0;
        }

        public void Add(object obj)
        {
            this.m_seg.Add(obj);
            if (this.MaxLength < obj.ToString().Length)
            {
                this.MaxLength = obj.ToString().Length;
            }
        }

        public object GetElem(int i)
        {
            if (i < this.Count)
            {
                return this.m_seg[i];
            }
            else
            {
                return null;
            }
        }

        public void SetElem(int i, object obj)
        {
            this.m_seg[i] = obj;
        }

        public bool Contains(object obj)
        {
            return this.m_seg.Contains(obj);
        }

        /// <summary>
        /// ����������
        /// </summary>
        public void Sort()
        {
            this.Sort(this);
        }

        /// <summary>
        /// ����������
        /// </summary>
        public void Sort(SegList list)
        {
            int max = 0;
            for (int i = 0; i < list.Count - 1; ++i)
            {
                max = i;
                for (int j = i + 1; j < list.Count; ++j)
                {
                    string str1 = list.GetElem(j).ToString();
                    string str2 = list.GetElem(max).ToString();
                    int l1;
                    int l2;
                    if (str1 == "null")
                    {
                        l1 = 0;
                    }
                    else
                    {
                        l1 = str1.Length;
                    }

                    if (str2 == "null")
                    {
                        l2 = 0;
                    }
                    else
                    {
                        l2 = str2.Length;
                    }

                    if (l1 > l2)
                    {
                        max = j;
                    }
                }
                object o = list.GetElem(max);
                list.SetElem(max, list.GetElem(i));
                list.SetElem(i, o);
            }
        }
    }

    /// <summary>
    /// �ִ���
    /// </summary>
    //----------------����----------------------
    //Segment seg = new Segment();
    //seg.InitWordDics();
    //seg.EnablePrefix = true;
    //seg.Separator =" ";
    //seg.SegmentText("�ַ���", false).Trim();
    //-------------------------------------------
    public class Segment
    {
        #region ˽���ֶ�

        private string m_DicPath = System.Web.HttpContext.Current.Server.MapPath("bin/ShootSeg/sDict.dic");

        private string m_NoisePath = System.Web.HttpContext.Current.Server.MapPath("bin/ShootSeg/sNoise.dic");

        private string m_NumberPath = System.Web.HttpContext.Current.Server.MapPath("bin/ShootSeg/sNumber.dic");

        private string m_WordPath = System.Web.HttpContext.Current.Server.MapPath("bin/ShootSeg/sWord.dic");

        private string m_PrefixPath = System.Web.HttpContext.Current.Server.MapPath("bin/ShootSeg/sPrefix.dic");

        private Hashtable htWords;

        private ArrayList alNoise;

        private ArrayList alNumber;

        private ArrayList alWord;

        private ArrayList alPrefix;

        private double m_EventTime = 0;

        /// <summary>
        /// �ָ���
        /// </summary>
        private string m_Separator = " ";

        /// <summary>
        /// ������֤���ֵ�������ʽ
        /// </summary>
        private string strChinese = "[\u4e00-\u9fa5]";

        #endregion ˽���ֶ�

        #region ��������

        /// <summary>
        /// �����ʵ�·��
        /// </summary>
        public string DicPath
        {
            get
            {
                return this.m_DicPath;
            }
            set
            {
                this.m_DicPath = value;
            }
        }

        /// <summary>
        /// ���ݻ��溯��
        /// </summary>
        /// <param name="key">������</param>
        /// <param name="val">���������</param>
        private static void SetCache(string key, object val)
        {
            if (val == null)
            {
                val = " ";
            }

            System.Web.HttpContext.Current.Application.Lock();
            System.Web.HttpContext.Current.Application.Set(key, val);
            System.Web.HttpContext.Current.Application.UnLock();
        }

        /// <summary>
        /// ��ȡ����
        /// </summary>
        private static object GetCache(string key)
        {
            return System.Web.HttpContext.Current.Application.Get(key);
        }

        /// <summary>
        /// ��ʱ����
        /// </summary>
        public string NoisePath
        {
            get
            {
                return this.m_NoisePath;
            }
            set
            {
                this.m_NoisePath = value;
            }
        }

        /// <summary>
        /// ���ִʵ�·��
        /// </summary>
        public string NumberPath
        {
            get
            {
                return this.m_NumberPath;
            }
            set
            {
                this.m_NumberPath = value;
            }
        }

        /// <summary>
        /// ��ĸ�ʵ�·��
        /// </summary>
        public string WordPath
        {
            get
            {
                return this.m_WordPath;
            }
            set
            {
                this.m_WordPath = value;
            }
        }

        /// <summary>
        /// ����ǰ׺�ֵ� ���ھ�������
        /// </summary>
        public string PrefixPath
        {
            get
            {
                return this.m_PrefixPath;
            }
            set
            {
                this.m_PrefixPath = value;
            }
        }

        /// <summary>
        /// �Ƿ�������������
        /// </summary>
        public bool EnablePrefix
        {
            get
            {
                if (this.alPrefix.Count == 0)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            set
            {
                if (value)
                {
                    this.alPrefix = this.LoadWords(this.PrefixPath, this.alPrefix);
                }
                else
                {
                    this.alPrefix = new ArrayList();
                }
            }
        }

        /// <summary>
        /// ��ʱÿ�ν��м��ػ�ִʶ���������Ա�ʾΪ��һ�ζ�������ʱ��
        /// �Ѿ�ȷ�����뵫�ִʲ������ַ����϶�ʱ����Ϊ0
        /// </summary>
        public double EventTime
        {
            get
            {
                return this.m_EventTime;
            }
        }

        /// <summary>
        /// �ָ���,Ĭ��Ϊ�ո�
        /// </summary>
        public string Separator
        {
            get
            {
                return this.m_Separator;
            }
            set
            {
                if (value != "" && value != null)
                {
                    this.m_Separator = value;
                }
            }
        }

        #endregion ��������

        #region ���췽��

        /// <summary>
        /// ���췽��
        /// </summary>
        public Segment()
        { }

        /// <summary>
        /// ���췽��
        /// </summary>
        public Segment(string p_DicPath, string p_NoisePath, string p_NumberPath, string p_WordPath)
        {
            this.m_WordPath = p_DicPath;
            this.m_WordPath = p_NoisePath;
            this.m_WordPath = p_NumberPath;
            this.m_WordPath = p_WordPath;
            this.InitWordDics();
        }

        #endregion ���췽��

        #region ���з���

        /// <summary>
        /// ���ش��б�
        /// </summary>
        public void InitWordDics()
        {
            DateTime start = DateTime.Now;
            if (GetCache("jcms_dict") == null)
            {
                this.htWords = new Hashtable();
                Hashtable father = this.htWords;
                Hashtable forfather = this.htWords;

                string strChar1;
                string strChar2;

                StreamReader reader = new StreamReader(this.DicPath, System.Text.Encoding.UTF8);
                string strline = reader.ReadLine();

                SegList list;
                Hashtable child = new Hashtable();

                long i = 0;
                while (strline != null && strline.Trim() != "")
                {
                    i++;
                    strChar1 = strline.Substring(0, 1);
                    strChar2 = strline.Substring(1, 1);
                    if (!this.htWords.ContainsKey(strChar1))
                    {
                        father = new Hashtable();
                        this.htWords.Add(strChar1, father);
                    }
                    else
                    {
                        father = (Hashtable)this.htWords[strChar1];
                    }

                    if (!father.ContainsKey(strChar2))
                    {
                        list = new SegList();
                        if (strline.Length > 2)
                        {
                            list.Add(strline.Substring(2));
                        }
                        else
                        {
                            list.Add("null");
                        }

                        father.Add(strChar2, list);
                    }
                    else
                    {
                        list = (SegList)father[strChar2];
                        if (strline.Length > 2)
                        {
                            list.Add(strline.Substring(2));
                        }
                        else
                        {
                            list.Add("null");
                        }
                        father[strChar2] = list;
                    }
                    this.htWords[strChar1] = father;
                    strline = reader.ReadLine();
                }
                try
                {
                    reader.Close();
                }
                catch
                { }
                SetCache("jcms_dict", this.htWords);
            }
            this.htWords = (Hashtable)GetCache("jcms_dict");

            this.alNoise = this.LoadWords(this.NoisePath, this.alNoise);
            this.alNumber = this.LoadWords(this.NumberPath, this.alNumber);
            this.alWord = this.LoadWords(this.WordPath, this.alWord);
            this.alPrefix = this.LoadWords(this.PrefixPath, this.alPrefix);

            TimeSpan duration = DateTime.Now - start;
            this.m_EventTime = duration.TotalMilliseconds;
        }

        /// <summary>
        /// �����ı����鵽ArrayList
        /// </summary>
        public ArrayList LoadWords(string strPath, ArrayList list)
        {
            StreamReader reader = new StreamReader(strPath, System.Text.Encoding.UTF8);
            list = new ArrayList();
            string strline = reader.ReadLine();
            while (strline != null)
            {
                list.Add(strline);
                strline = reader.ReadLine();
            }
            try
            {
                reader.Close();
            }
            catch
            { }
            return list;
        }

        /// <summary>
        /// ������б�
        /// </summary>
        public void OutWords()
        {
            IDictionaryEnumerator idEnumerator1 = this.htWords.GetEnumerator();
            while (idEnumerator1.MoveNext())
            {
                IDictionaryEnumerator idEnumerator2 = ((Hashtable)idEnumerator1.Value).GetEnumerator();
                while (idEnumerator2.MoveNext())
                {
                    SegList aa = (SegList)idEnumerator2.Value;
                    for (int i = 0; i < aa.Count; i++)
                    {
                        Console.WriteLine(idEnumerator1.Key.ToString() + idEnumerator2.Key.ToString() + aa.GetElem(i).ToString());
                    }
                }
            }
        }

        /// <summary>
        /// ���ArrayList
        /// </summary>
        public void OutArrayList(ArrayList list)
        {
            if (list == null)
            {
                return;
            }

            for (int i = 0; i < list.Count; i++)
            {
                Console.WriteLine(list[i].ToString());
            }
        }

        /// <summary>
        /// �ִʹ���,��֧�ֻس�
        /// </summary>
        /// <param name="strText">Ҫ�ִʵ��ı�</param>
        /// <returns>�ִʺ���ı�</returns>
        public string SegmentText(string strText)
        {
            strText = (strText + "$").Trim();
            if (this.htWords == null)
            {
                return strText;
            }

            if (strText.Length < 3)
            {
                return strText;
            }

            DateTime start = DateTime.Now;
            int length = 0;
            int preFix = 0;
            bool word = false;
            bool number = false;
            string reText = "";
            string strPrefix = "";
            string strLastChar = "";
            string strLastWords = this.Separator;

            for (int i = 0; i < strText.Length - 1; i++)
            {
                #region ����ÿһ���ֵĴ������

                string strChar1 = strText.Substring(i, 1);
                string strChar2 = strText.Substring(i + 1, 1).Trim();
                bool yes;
                SegList l;
                Hashtable h;

                if (reText.Length > 0)
                {
                    strLastChar = reText.Substring(reText.Length - 1);
                }

                if (strChar1 == " ")
                {
                    if ((number || word) && strLastChar != this.Separator)
                    {
                        reText += this.Separator;
                    }

                    yes = true;
                }
                else
                {
                    yes = false;
                }

                int CharType = this.GetCharType(strChar1);
                switch (CharType)
                {
                    case 1:

                        #region ��������֣�������ֵ���һλ����ĸҪ�ͺ�������ַֿ�

                        if (word)
                        {
                            reText += this.Separator;
                        }
                        word = false;
                        number = true;
                        strLastWords = "";
                        break;

                    #endregion ��������֣�������ֵ���һλ����ĸҪ�ͺ�������ַֿ�

                    case 2:
                    case 5:

                        #region �������ĸ

                        if (number)
                        {
                            strLastWords = this.Separator;
                        }
                        else
                        {
                            strLastWords = "";
                        }

                        word = true;
                        number = false;
                        break;

                    #endregion �������ĸ

                    case 3:
                    case 4:

                        #region ��һ����ϣ���Ƿ�����ؼ��֣������������ڶ�����ϣ��

                        //��һ�����Ƿ�Ϊ��ĸ
                        if (word)
                        {
                            reText += this.Separator;
                        }

                        #region �����һ���Ƿ������֣���������������������ֺ�����ʵ�

                        if (number && CharType != 4)
                        {
                            h = (Hashtable)this.htWords["n"];
                            if (h.ContainsKey(strChar1))
                            {
                                l = (SegList)h[strChar1];
                                if (l.Contains(strChar2))
                                {
                                    reText += strChar1 + strChar2 + this.Separator;
                                    yes = true;
                                    i++;
                                }
                                else if (l.Contains("null"))
                                {
                                    reText += strChar1 + this.Separator;
                                    yes = true;
                                }
                            }
                            else
                            {
                                reText += this.Separator;
                            }
                        }

                        #endregion �����һ���Ƿ������֣���������������������ֺ�����ʵ�

                        //�Ǻ������ֵĺ���
                        if (CharType == 3)
                        {
                            word = false;
                            number = false;
                            strLastWords = this.Separator;
                        }
                        else
                        {
                            word = false;
                            number = true;
                            strLastWords = "";
                        }

                        //�ڶ�����ϣ��ȡ��
                        h = (Hashtable)this.htWords[strChar1];

                        //�ڶ�����ϣ���Ƿ�����ؼ���
                        if (h.ContainsKey(strChar2))
                        {
                            #region �ڶ��������ؼ���

                            //ȡ��ArrayList����
                            l = (SegList)h[strChar2];

                            //����ÿһ������ ���Ƿ�����ϳɴ�
                            for (int j = 0; j < l.Count; j++)
                            {
                                bool have = false;
                                string strChar3 = l.GetElem(j).ToString();

                                //����ÿһ��ȡ���Ĵʽ��м��,���Ƿ�ƥ�䣬���ȱ���
                                if ((strChar3.Length + i + 2) < strText.Length)
                                {
                                    //��i+2��ȡ��m���ȵ���
                                    string strChar = strText.Substring(i + 2, strChar3.Length).Trim();
                                    if (strChar3 == strChar && !yes)
                                    {
                                        if (strPrefix != "")
                                        {
                                            reText += strPrefix + this.Separator;
                                            strPrefix = "";
                                            preFix = 0;
                                        }
                                        reText += strChar1 + strChar2 + strChar;
                                        i += strChar3.Length + 1;
                                        have = true;
                                        yes = true;
                                        break;
                                    }
                                }
                                else if ((strChar3.Length + i + 2) == strText.Length)
                                {
                                    string strChar = strText.Substring(i + 2).Trim();
                                    if (strChar3 == strChar && !yes)
                                    {
                                        if (strPrefix != "")
                                        {
                                            reText += strPrefix + this.Separator;
                                            strPrefix = "";
                                            preFix = 0;
                                        }
                                        reText += strChar1 + strChar2 + strChar;
                                        i += strChar3.Length + 1;
                                        have = true;
                                        yes = true;
                                        break;
                                    }
                                }

                                if (!have && j == l.Count - 1 && l.Contains("null") && !yes)
                                {
                                    if (preFix == 1)
                                    {
                                        reText += strPrefix + strChar1 + strChar2;
                                        strPrefix = "";
                                        preFix = 0;
                                    }
                                    else if (preFix > 1)
                                    {
                                        reText += strPrefix + strLastWords + strChar1 + strChar2;
                                        strPrefix = "";
                                        preFix = 0;
                                    }
                                    else
                                    {
                                        if (CharType == 4)
                                        {
                                            reText += strChar1 + strChar2;
                                        }
                                        else
                                        {
                                            reText += strChar1 + strChar2;
                                        }

                                        strLastWords = this.Separator;
                                        number = false;
                                    }
                                    i++;
                                    yes = true;
                                    break;
                                }
                                else if (have)
                                {
                                    break;
                                }
                            }

                            #endregion �ڶ��������ؼ���

                            //���û��ƥ�仹������һ��������������ֻ�������֣����������ֿ�ͷ�Ĵ��ﲻ����
                            if (!yes && l.Contains("null"))
                            {
                                if (preFix == 1)
                                {
                                    reText += strPrefix + strChar1 + strChar2;
                                    strPrefix = "";
                                    preFix = 0;
                                }
                                else if (preFix > 1)
                                {
                                    reText += strPrefix + strLastWords + strChar1 + strChar2;
                                    strPrefix = "";
                                    preFix = 0;
                                }
                                else
                                {
                                    if (CharType == 4)
                                    {
                                        reText += strChar1 + strChar2;
                                    }
                                    else
                                    {
                                        reText += strChar1 + strChar2;
                                    }

                                    strLastWords = this.Separator;
                                    number = false;
                                }
                                i++;
                                yes = true;
                            }
                            if (reText.Length > 0)
                            {
                                strLastChar = reText.Substring(reText.Length - 1);
                            }

                            if (CharType == 4 && this.GetCharType(strLastChar) == 4)
                            {
                                number = true;
                            }
                            else if (strLastChar != this.Separator)
                            {
                                reText += this.Separator;
                            }
                        }

                        #endregion ��һ����ϣ���Ƿ�����ؼ��֣������������ڶ�����ϣ��

                        break;

                    default:

                        #region δ֪�ַ�,��������Ƨ��,Ҳ�����Ǳ�����֮��

                        if (word && !yes)
                        {
                            reText += this.Separator;
                        }
                        else if (number && !yes)
                        {
                            reText += this.Separator;
                        }
                        number = false;
                        word = false;
                        strLastWords = this.Separator;
                        break;

                        #endregion δ֪�ַ�,��������Ƨ��,Ҳ�����Ǳ�����֮��
                }
                if (!yes && number || !yes && word)
                {
                    reText += strChar1;
                    yes = true;
                }
                if (!yes)
                {
                    #region ������������

                    if (preFix == 0)
                    {
                        if (this.alPrefix.Contains(strChar1 + strChar2))
                        {
                            i++;
                            strPrefix = strChar1 + strChar2;
                            preFix++;
                        }
                        else if (this.alPrefix.Contains(strChar1))
                        {
                            if (!number)
                            {
                                strPrefix = strChar1;
                                preFix++;
                            }
                            else
                            {
                                reText += strChar1 + strLastWords;
                                number = false;
                                word = false;
                            }
                        }
                        else
                        {
                            if (preFix == 3)
                            {
                                reText += strPrefix + this.Separator + strChar1 + this.Separator;
                                strPrefix = "";
                                preFix = 0;
                            }
                            else if (preFix > 0)
                            {
                                if (Regex.IsMatch(strChar1, this.strChinese))
                                {
                                    strPrefix += strChar1;
                                    preFix++;
                                }
                                else
                                {
                                    reText += strPrefix + this.Separator + strChar1 + this.Separator;
                                    strPrefix = "";
                                    preFix = 0;
                                }
                            }
                            else
                            {
                                reText += strChar1 + strLastWords;
                                number = false;
                                word = false;
                            }
                        }
                    }
                    else
                    {
                        if (preFix == 3)
                        {
                            reText += strPrefix + this.Separator + strChar1 + this.Separator;
                            strPrefix = "";
                            preFix = 0;
                        }
                        else if (preFix > 0)
                        {
                            if (Regex.IsMatch(strChar1, this.strChinese))
                            {
                                strPrefix += strChar1;
                                preFix++;
                            }
                            else
                            {
                                reText += strPrefix + this.Separator + strChar1 + this.Separator;
                                strPrefix = "";
                                preFix = 0;
                            }
                        }
                        else
                        {
                            reText += strChar1 + strLastWords;
                            number = false;
                        }
                    }

                    #endregion ������������
                }
                length = i;

                #endregion ����ÿһ���ֵĴ������
            }

            #region ����ֹ���һ���ֵĶ�ʧ

            if (length < strText.Length - 1)
            {
                string strLastChar1 = strText.Substring(strText.Length - 1).Trim();
                string strLastChar2 = strText.Substring(strText.Length - 2).Trim();

                if (reText.Length > 0)
                {
                    strLastChar = reText.Substring(reText.Length - 1);
                }

                if (preFix != 0)
                {
                    reText += strPrefix + strLastChar1;
                }
                else
                {
                    switch (this.GetCharType(strLastChar1))
                    {
                        case 1:
                            if (strLastChar1 != "." && strLastChar1 != "��")
                            {
                                reText += strLastChar1;
                            }
                            else
                            {
                                reText += this.Separator + strLastChar1;
                            }

                            break;

                        case 2:
                        case 5:
                            if (this.alWord.Contains(strLastChar2))
                            {
                                reText += strLastChar1;
                            }

                            break;

                        case 3:
                        case 4:
                            if ((number || word) && strLastChar != this.Separator)
                            {
                                reText += this.Separator + strLastChar1;
                            }
                            else
                            {
                                reText += strLastChar1;
                            }

                            break;

                        default:
                            if (strLastChar != this.Separator)
                            {
                                reText += this.Separator + strLastChar1;
                            }
                            else
                            {
                                reText += strLastChar1;
                            }

                            break;
                    }
                }
                if (reText.Length > 0)
                {
                    strLastChar = (reText.Substring(reText.Length - 1));
                }

                if (strLastChar != this.Separator)
                {
                    reText += this.Separator;
                }
            }

            #endregion ����ֹ���һ���ֵĶ�ʧ

            TimeSpan duration = DateTime.Now - start;
            this.m_EventTime = duration.TotalMilliseconds;
            return reText.Replace(" $", ""); //�������һ���ֵģ���ȥ��
        }

        /// <summary>
        /// ���طִʹ���,֧�ֻس�
        /// </summary>
        public string SegmentText(string strText, bool Enter)
        {
            if (Enter)
            {
                DateTime start = DateTime.Now;
                string[] strArr = strText.Split('\n');

                string reText = "";
                for (int i = 0; i < strArr.Length; i++)
                {
                    reText += this.SegmentText(strArr[i]) + "\r\n";
                }

                TimeSpan duration = DateTime.Now - start;
                this.m_EventTime = duration.TotalMilliseconds;
                return reText;
            }
            else
            {
                return this.SegmentText(strText);
            }
        }

        #region �ж��ַ�����

        /// <summary>
        /// �ж��ַ�����,0Ϊδ֪,1Ϊ����,2Ϊ��ĸ,3Ϊ����,4Ϊ��������
        /// </summary>
        private int GetCharType(string p_Char)
        {
            int CharType = 0;
            if (this.alNumber.Contains(p_Char))
            {
                CharType = 1;
            }

            if (this.alWord.Contains(p_Char))
            {
                CharType = 2;
            }

            if (this.htWords.ContainsKey(p_Char))
            {
                CharType += 3;
            }

            return CharType;
        }

        #endregion �ж��ַ�����

        #region �Լ��صĴʵ���������д��

        /// <summary>
        /// �Լ��صĴʵ���������д��
        /// </summary>
        public void SortDic()
        {
            this.SortDic(false);
        }

        /// <summary>
        /// �Լ��صĴʵ���������д��
        /// </summary>
        /// <param name="Reload">�Ƿ����¼���</param>
        public void SortDic(bool Reload)
        {
            DateTime start = DateTime.Now;
            StreamWriter sw = new StreamWriter(this.DicPath, false, System.Text.Encoding.UTF8);

            IDictionaryEnumerator idEnumerator1 = this.htWords.GetEnumerator();
            while (idEnumerator1.MoveNext())
            {
                IDictionaryEnumerator idEnumerator2 = ((Hashtable)idEnumerator1.Value).GetEnumerator();
                while (idEnumerator2.MoveNext())
                {
                    SegList aa = (SegList)idEnumerator2.Value;
                    aa.Sort();
                    for (int i = 0; i < aa.Count; i++)
                    {
                        if (aa.GetElem(i).ToString() == "null")
                        {
                            sw.WriteLine(idEnumerator1.Key.ToString() + idEnumerator2.Key.ToString());
                        }
                        else
                        {
                            sw.WriteLine(idEnumerator1.Key.ToString() + idEnumerator2.Key.ToString() + aa.GetElem(i).ToString());
                        }
                    }
                }
            }
            sw.Close();

            if (Reload)
            {
                this.InitWordDics();
            }

            TimeSpan duration = DateTime.Now - start;
            this.m_EventTime = duration.TotalMilliseconds;
        }

        #endregion �Լ��صĴʵ���������д��

        /// <summary>
        /// ɾ��������ȫ��ͬ�Ĵ�,��ʱ����!
        /// </summary>
        /// <returns>��ͬ��������</returns>
        public int Optimize()
        {
            int l = 0;
            DateTime start = DateTime.Now;

            Hashtable htOptimize = new Hashtable();
            StreamReader reader = new StreamReader(this.DicPath, System.Text.Encoding.UTF8);
            string strline = reader.ReadLine();
            while (strline != null && strline.Trim() != "")
            {
                if (!htOptimize.ContainsKey(strline))
                {
                    htOptimize.Add(strline, null);
                }
                else
                {
                    l++;
                }
            }
            Console.WriteLine("ready");
            try
            {
                reader.Close();
            }
            catch { }
            StreamWriter sw = new StreamWriter(this.DicPath, false, System.Text.Encoding.UTF8);
            IDictionaryEnumerator ide = htOptimize.GetEnumerator();
            while (ide.MoveNext())
            {
                sw.WriteLine(ide.Key.ToString());
            }

            try
            {
                sw.Close();
            }
            catch { }
            TimeSpan duration = DateTime.Now - start;
            this.m_EventTime = duration.TotalMilliseconds;
            return l;
        }

        #endregion ���з���
    }
}