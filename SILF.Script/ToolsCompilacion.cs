using Microsoft.VisualBasic;

namespace SILF.Script
{

    public static partial class Compilacion
    {




        public static void Folders(string folderPath)
        {

            if (Directory.Exists(folderPath) == false)
                Directory.CreateDirectory(folderPath);

            if (Directory.Exists(folderPath + @"functions\") == false)
                Directory.CreateDirectory(folderPath + @"functions\");

            if (Directory.Exists(folderPath + @"spaces\") == false)
                Directory.CreateDirectory(folderPath + @"spaces\");

            if (Directory.Exists(folderPath + @"ifs\") == false)
                Directory.CreateDirectory(folderPath + @"ifs\");

            if (Directory.Exists(folderPath + @"do\") == false)
                Directory.CreateDirectory(folderPath + @"do\");

            if (Directory.Exists(folderPath + @"while\") == false)
                Directory.CreateDirectory(folderPath + @"while\");

            if (Directory.Exists(folderPath + @"design\") == false)
                Directory.CreateDirectory(folderPath + @"design\");

            if (Directory.Exists(folderPath + @"build\") == false)
                Directory.CreateDirectory(folderPath + @"build\");

        }


        public static void GetMain(RichTextBox Codigo, List<string> main)
        {
            MessageBox.Show(Codigo.Text);
            try
            {
                bool BN = false;
                foreach (string Lineas in Codigo.Lines)
                {
                    string Linea = Lineas;

                    if (Linea.Trim().ToLower() == "space main:")
                    {
                        BN = true;
                    }

                    else if (Linea.Trim().ToLower() == "end space")
                    {
                        BN = false;
                    }


                    else if (BN == true)
                    {
                        Linea = Linea.Trim();
                        if (!string.IsNullOrEmpty(Linea))
                        {
                            main.Add(Linea);
                        }

                    }
                }
            }
            catch
            {
            }

        }





        public static List<string> GetIf(List<string> Codigo, string folderpath)
        {

            //Lista donde se almacenan los URL de los Ifs
            List<string> ListaIfs = new();

            //Lista donde esta el codigo ya modificado
            List<string> Modified = new();

            //Interprete de If
            {
                //Variables necesarias

                string name = ""; //Nombre del If
                string Location = ""; //Ubicacion del If

                string txtAntes = ""; //Se recoje antes del else
                string txtElse = "";  //Se recoje despues del else

                bool Bander = false; //Si esta o no recojiendo un If
                bool Bander2 = true; //Si esta en un Else


                int cr = 0;
                foreach (string dd in Codigo)
                {
                    string d = dd.Trim();


                    if (d.LowStartsWith("if") & d.EndsWith(":"))
                    {
                        cr += 1;
                        if (cr == 1)
                        {
                            Bander = true;

                            string aux = d.Remove(0, d.IndexOf("("));
                            name = aux.ExtractFrom("(", ")");
                            name = $"[{name}]";

                            Location = (300000 * VBMath.Rnd() + 1).ToString();

                            d = "";
                        }
                    }


                    else if (d.LowStartsWith("end if"))
                    {
                        cr -= 1;
                        if (cr == 0)
                        {
                            Bander = false;
                            string SV = "";
                            SV = ":nn:" + Environment.NewLine + txtAntes + ":else:" + Environment.NewLine + txtElse;
                            ListaIfs.Add(folderpath + @"ifs\" + Location + ".sfc");
                            File.WriteAllText(folderpath + @"ifs\" + Location + ".sfc", SV);
                            Modified.Add("execute|if|" + Location + "|" + name);

                            Bander = false;
                            Bander2 = true;
                            name = "";
                            txtAntes = "";
                            txtElse = "";
                            Location = "";
                            d = "";

                        }
                    }


                    else if (d.ToLower() == "else" & cr == 1)
                    {
                        Bander2 = false;
                        d = "";
                    }


                    if (Bander == true)
                    {

                        switch (Bander2)
                        {
                            case true:
                                {
                                    if (d != null & d != "")
                                        txtAntes = txtAntes + d + Environment.NewLine;
                                    break;
                                }

                            case false:
                                {
                                    if (d != null & d != "")
                                        txtElse = txtElse + d + Environment.NewLine;
                                    break;
                                }
                        }
                    }


                    else
                        Modified.Add(d);

                }



                //Si hay mas Ifs los lleva denuevo al interprete de Ifs
                if (ListaIfs.Count > 0)
                    foreach (string C in ListaIfs)
                        GetIf2(C, folderpath + @"ifs\");

            }


            return Modified;
        }

        public static void GetIf2(string Filer, string General)
        {

            //Lista de codigos
            List<string> codigo = new();

            //Agrega los archivos a la lista
            foreach (var LINEA in File.ReadLines(Filer))
                codigo.Add(LINEA);

            //Lista del archivo actual
            List<string> actual = new();

            //Lista de otros Ifs
            List<string> ListaIfs = new();


            //Extraccion de datos
            {
                string IFname = "";
                string IFcont = "";
                bool Bander = false;
                string IFcontELSE = "";
                bool Bander2 = true;
                string IFubi = "";

                int cr = 0;
                foreach (string dd in codigo)
                {
                    string d = dd;

                    if (d.LowStartsWith("if") & d.ToLower().EndsWith(":"))
                    {
                        cr += 1;
                        if (cr == 1)
                        {
                            Bander = true;

                            string aux = d.Remove(0, d.IndexOf("("));
                            IFname = aux.ExtractFrom("(", ")");
                            IFname = $"[{IFname}]";

                            IFubi = (300000 * VBMath.Rnd() + 1).ToString();
                            d = "";
                        }
                    }


                    else if (d.ToLower().Replace(" ", "") == "endif")
                    {
                        cr -= 1;
                        if (cr == 0)
                        {
                            Bander = false;
                            string SV = "";
                            SV = ":nn:" + Environment.NewLine + IFcont + ":else:" + Environment.NewLine + IFcontELSE;
                            ListaIfs.Add(General + IFubi.ToString() + ".sfc");

                            File.WriteAllText(General + IFubi.ToString() + ".sfc", SV);
                            actual.Add("execute|if|" + IFubi.ToString() + "|" + IFname);

                            Bander = false;
                            Bander2 = true;
                            IFname = "";
                            IFcont = "";
                            IFcontELSE = "";
                            IFubi = 0.ToString();
                            d = "";

                        }
                    }


                    else if (d.ToLower().Replace(" ", "") == "else" & cr == 1)
                    {
                        Bander2 = false;
                        d = "";

                    }


                    if (Bander == true)
                    {
                        if (Bander2 == true)
                            if (d != null | d.Trim() != "")
                                IFcont = IFcont + d + Environment.NewLine;

                            else if (d != null | d.Trim() != "")
                                IFcontELSE = IFcontELSE + d + Environment.NewLine;
                    }

                    else
                        actual.Add(d);

                }


            }


            StringBuilder save = new();
            foreach (string t in actual)
                save.Append(t + Environment.NewLine);

            //Guarda el archivo
            File.WriteAllText(Filer, save.ToString());



            if (ListaIfs.Count > 0)
                foreach (var C in ListaIfs)
                    GetIf2(C, General);

        }



        public static List<string> GetFOR(List<string> List, string folderpath)
        {

            var rt = new List<string>();
            var FORS = new List<string>();

            string FORname = "";
            string FORvar = "";
            string ForTipo = "";
            string FORcont = "";
            bool Bander = false;
            string DOubi = "";

            int cr = 0;
            foreach (string dd in List)
            {
                string d = dd;

                if (d.ToLower().StartsWith("for") & d.ToLower().EndsWith(":"))
                {
                    cr += 1;
                    if (cr == 1)
                    {
                        Bander = true;
                        FORvar = d.Split(' ')[1];
                        if (d.Split(' ')[2].ToLower() == "in")
                        {
                            ForTipo = "iter";
                        }
                        else
                        {
                            ForTipo = "rep";
                        }


                        string aux = d.Remove(0, d.IndexOf("("));
                        FORname = aux.ExtractFrom("(", ")");
                        FORname = $"[{FORname}]";

                        DOubi = (300000 * VBMath.Rnd() + 1).ToString();
                        d = "";
                    }
                }


                else if (d == "next")
                {
                    cr -= 1;
                    if (cr == 0)
                    {
                        Bander = false;
                        string SV = "";
                        SV = FORcont;
                        FORS.Add(folderpath + @"do\" + DOubi.ToString() + ".sfc");
                        File.WriteAllText(folderpath + @"do\" + DOubi.ToString() + ".sfc", SV);
                        rt.Add("execute|do|" + DOubi.ToString() + "|" + ForTipo + "|" + FORvar + "|" + FORname);

                        Bander = false;
                        FORname = "";
                        FORcont = "";
                        DOubi = 0.ToString();
                        d = "";

                    }


                }


                if (Bander == true)
                {

                    if (string.IsNullOrEmpty(d.Replace(" ", "")))
                    {
                    }
                    else
                    {
                        FORcont = FORcont + d + Environment.NewLine;
                    }
                }
                else
                {
                    rt.Add(d);
                }

            }

            // ----------------------------IFS------------------------------------------------
            var ON_IF = new List<string>();

            var ss = new DirectoryInfo(folderpath + "ifs/");
            ss.GetFiles();

            foreach (var s in ss.GetFiles())
                ON_IF.Add(s.FullName);

            if (ON_IF.Count == 0)
            {
            }
            else
            {
                foreach (var C in ON_IF)
                    GetFOR2(C, folderpath + "do/");
            }
            // ----------------------------------------------------------------------------



            if (FORS.Count == 0)
            {
            }
            else
            {
                foreach (var C in FORS)
                    GetFOR2(C, folderpath + @"do\");
            }


            return rt;
        }

        public static void GetFOR2(string Filer, string General)
        {

            var list = new List<string>();
            foreach (var LINEA in File.ReadLines(Filer))
                list.Add(LINEA);

            var rt = new List<string>();
            var FORS = new List<string>();
            string ForVar = "";
            string FORname = "";
            string FORcont = "";
            bool Bander = false;
            string Fortipo = "";
            string FORubi = "";

            int cr = 0;
            foreach (string dd in list)
            {
                string d = dd;

                if (d.ToLower().StartsWith("for") & d.ToLower().EndsWith(":"))
                {
                    cr += 1;
                    if (cr == 1)
                    {
                        Bander = true;
                        ForVar = d.Split(' ')[1];

                        string aux = d.Remove(0, d.IndexOf("("));
                        FORname = aux.ExtractFrom("(", ")");
                        FORname = $"[{FORname}]";


                        if (d.Split(' ')[2].ToLower() == "in")
                        {
                            Fortipo = "iter";
                        }
                        else
                        {
                            Fortipo = "rep";
                        }

                        FORubi = (300000 * VBMath.Rnd() + 1).ToString();
                        d = "";
                    }
                }


                else if (d == "next")
                {
                    cr -= 1;
                    if (cr == 0)
                    {
                        Bander = false;
                        string SV = "";
                        SV = FORcont;
                        FORS.Add(General + FORubi.ToString() + ".sfc");
                        File.WriteAllText(General + FORubi.ToString() + ".sfc", SV);
                        rt.Add("execute|do|" + FORubi.ToString() + "|" + Fortipo + "|" + ForVar + "|" + FORname);
                        Bander = false;

                        FORname = "";
                        FORcont = "";

                        FORubi = 0.ToString();
                        d = "";

                    }



                }


                if (Bander == true)
                {
                    if (string.IsNullOrEmpty(d.Replace(" ", "")))
                    {
                    }
                    else
                    {
                        FORcont = FORcont + d + Environment.NewLine;
                    }
                }
                else
                {
                    rt.Add(d);
                }

            }


            string save = "";

            int q = 0;
            foreach (var t in rt)
            {
                if (q == rt.Count - 1)
                {
                    save += t;
                }
                else
                {
                    save += t + Environment.NewLine;
                }

                q += 1;
            }

            try
            {
                File.WriteAllText(Filer, save);
            }
            catch (Exception ex)
            {

            }



            if (FORS.Count == 0)
            {
            }
            else
            {
                foreach (var C in FORS)
                    GetIf2(C, General);
            }



        }



        public static List<string> GetWHILE(List<string> List, string folderpath)
        {

            var rt = new List<string>();
            var FORS = new List<string>();

            string WHILEname = "";

            string WHILEcont = "";
            bool Bander = false;
            string WHILEubi = "";

            int cr = 0;
            foreach (string dd in List)
            {
                string d = dd;

                if (d.ToLower().StartsWith("while") & d.ToLower().EndsWith(":"))
                {
                    cr += 1;
                    if (cr == 1)
                    {
                        Bander = true;
                        WHILEname = d.Remove(0, d.IndexOf("["));
                        WHILEname = WHILEname.Reverse();
                        WHILEname = WHILEname.Remove(0, WHILEname.IndexOf("]"));
                        WHILEname = WHILEname.Reverse();
                        WHILEubi = (300000 * VBMath.Rnd() + 1).ToString();
                        d = "";
                    }
                }


                else if (d == "end while")
                {
                    cr -= 1;
                    if (cr == 0)
                    {
                        Bander = false;
                        string SV = "";
                        SV = WHILEcont;
                        FORS.Add(folderpath + @"while\" + WHILEubi.ToString() + ".sfc");
                        Microsoft.VisualBasic.FileIO.FileSystem.WriteAllText(folderpath + @"while\" + WHILEubi.ToString() + ".sfc", SV, false);
                        rt.Add("execute|while|" + WHILEubi.ToString() + "|" + WHILEname);

                        Bander = false;
                        WHILEname = "";
                        WHILEcont = "";
                        WHILEubi = 0.ToString();
                        d = "";

                    }


                }


                if (Bander == true)
                {
                    if (string.IsNullOrEmpty(d.Replace(" ", "")))
                    {
                    }
                    else
                    {
                        WHILEcont = WHILEcont + d + Environment.NewLine;
                    }
                }
                else
                {
                    rt.Add(d);
                }

            }

            // ----------------------------IFS------------------------------------------------
            var ON_IF = new List<string>();
            foreach (var s in Microsoft.VisualBasic.FileIO.FileSystem.GetFiles(folderpath + @"ifs\"))
                ON_IF.Add(s);
            if (ON_IF.Count == 0)
            {
            }
            else
            {
                foreach (var C in ON_IF)
                    GetWHILE2(C, folderpath + @"while\");
            }
            // ----------------------------------------------------------------------------

            // ----------------------------FOR------------------------------------------------
            var ON_DO = new List<string>();
            foreach (var s in Microsoft.VisualBasic.FileIO.FileSystem.GetFiles(folderpath + @"do\"))
                ON_DO.Add(s);
            if (ON_DO.Count == 0)
            {
            }
            else
            {
                foreach (var C in ON_DO)
                    GetWHILE2(C, folderpath + @"while\");
            }
            // ----------------------------------------------------------------------------



            if (FORS.Count == 0)
            {
            }
            else
            {
                foreach (var C in FORS)
                    GetWHILE2(C, folderpath + "while/");
            }


            return rt;
        }

        public static void GetWHILE2(string Filer, string General)
        {

            var list = new List<string>();
            foreach (var LINEA in File.ReadLines(Filer))
                list.Add(LINEA);

            var rt = new List<string>();
            var FORS = new List<string>();

            string FORname = "";
            string FORcont = "";
            bool Bander = false;

            string FORubi = "";

            int cr = 0;
            foreach (string dd in list)
            {
                string d = dd;

                if (d.ToLower().StartsWith("while") & d.ToLower().EndsWith(":"))
                {
                    cr += 1;
                    if (cr == 1)
                    {
                        Bander = true;
                        FORname = d.Remove(0, d.IndexOf("["));
                        FORname = FORname.Reverse();
                        FORname = FORname.Remove(0, FORname.IndexOf("]"));
                        FORname = FORname.Reverse();
                        FORubi = (300000 * VBMath.Rnd() + 1).ToString();
                        d = "";
                    }
                }


                else if (d == "end while")
                {
                    cr -= 1;
                    if (cr == 0)
                    {
                        Bander = false;
                        string SV = "";
                        SV = FORcont;
                        FORS.Add(General + FORubi.ToString() + ".sfc");
                        Microsoft.VisualBasic.FileIO.FileSystem.WriteAllText(General + FORubi.ToString() + ".sfc", SV, false);
                        rt.Add("execute|while|" + FORubi.ToString() + "|" + FORname);
                        Bander = false;

                        FORname = "";
                        FORcont = "";

                        FORubi = 0.ToString();
                        d = "";

                    }



                }


                if (Bander == true)
                {
                    if (string.IsNullOrEmpty(d.Replace(" ", "")))
                    {
                    }
                    else
                    {
                        FORcont = FORcont + d + Environment.NewLine;
                    }
                }
                else
                {
                    rt.Add(d);
                }

            }


            string save = "";

            foreach (var t in rt)
                save = save + t.ToString() + Environment.NewLine;

            try
            {
                File.WriteAllText(Filer, save);
            }
            catch (Exception ex)
            {

            }



            if (FORS.Count == 0)
            {
            }
            else
            {
                foreach (var C in FORS)
                    GetWHILE2(C, General);
            }



        }


    }

}