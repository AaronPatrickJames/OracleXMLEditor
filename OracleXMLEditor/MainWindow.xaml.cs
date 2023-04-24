using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Excel = Microsoft.Office.Interop.Excel;
using System.Xml;
using Microsoft.Win32;

namespace OracleXMLEditor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        //Read in document from path
        public XmlDocument XMLLoader( string myPath)
        {
            XmlDocument mydoc = new XmlDocument();
            mydoc.Load(myPath);

            return mydoc;
        }

        //get document that edits
        public List<string[]> getEditorxmls(string currentFilePath)
        {
            Excel.Application oExcel = new Excel.Application();

            string editorFilePath = currentFilePath;

            //pass that to workbook object  
            Excel.Workbook WB = oExcel.Workbooks.Open(editorFilePath);

            //get the workbookname  
            string ExcelWorkbookname = WB.Name;

            //get the worksheet count  
            int worksheetcount = WB.Worksheets.Count;


            //Get first Worksheet
            Excel.Worksheet wks = (Excel.Worksheet)WB.Worksheets[1];

            // statement get the firstworksheetname  
            string firstworksheetname = wks.Name;

            //statement get the first cell value  
            //var firstcellvalue = (string)((Excel.Range)wks.Cells[1, 1]).Value;

            //debug stuff only 
            //Debug.WriteLine(firstcellvalue);

            //Create new find last object
            FindLastValue myNewHolder = new FindLastValue();
            myNewHolder.totalRows = myNewHolder.rows(wks);
            myNewHolder.totalColumns = myNewHolder.collums(wks);

            //Debug.WriteLine(myNewHolder.totalRows.ToString());
            //Debug.WriteLine(myNewHolder.totalColumns.ToString());

            int totalrows = myNewHolder.totalRows;
            int totalcol = myNewHolder.totalColumns;
            //Create Array
            List<string[]> myUpdateList = new List<string[]>();

            for (int i = 0;i < totalrows;i++)
            {
                string[] values = new string[totalcol];
                
                for(int j = 0; j < totalcol; j++)
                {
                    Debug.WriteLine(j);
                    Debug.WriteLine((string)((Excel.Range)wks.Cells[i + 1, j + 1]).Value);
                    values[j] = (string)((Excel.Range)wks.Cells[i+1, j+1]).Value; // add one because arrays start at 0, but excel files start at 1
                }
                //Add Values to List
                myUpdateList.Add(values);
            }
            //Close excel - to be used again
            WB.Close();

            return myUpdateList;
        }

        public XmlDocument getEditableDocument(string editableFilePath)
        {
            //Get Editable Document
            XmlDocument myeditableXML = XMLLoader(editableFilePath);

            return myeditableXML;
        }

        //string[,] myArray
        public void EditDocument(XmlDocument myEditable, List<string[]> myEditor, string savePath)
        {

            //Get all Nodes under tag "Category"
            XmlNodeList aCompareableNodes = myEditable.SelectNodes("/CategoryVO/CategoryVORow");


            //Editor for each
            foreach(var lineItem in myEditor){ 

                //for each item in list
                foreach (XmlNode aNodes in aCompareableNodes)
                {
                    // for each element in that list
                    foreach (XmlNode aNode in aNodes)
                    {
                        if (aNode.Name == "Category")
                        {
                            string aNodeWhereClause = aNode.InnerText;

                            //Check key value list to find where aNodeListCheck-Key is equal to Value 

                            if (aNodeWhereClause == lineItem[0].ToString())
                            {

                                //Check the Sub Node
                                foreach (XmlNode aSubNode in aNodes)
                                {
                                    if (aSubNode.Name == "CategoryBook")
                                    {
                                        foreach (XmlNode aCatBookNode in aSubNode)
                                        {

                                            if (aCatBookNode.Name == "CategoryBookVORow")
                                            {

                                                foreach (XmlNode aCategoryBookDefaultNode in aCatBookNode)
                                                {
                                                    if (aCategoryBookDefaultNode.Name == "CategoryBookDefault")
                                                    {
                                                        foreach (XmlNode aCategoryBookDefaultVORowNode in aCategoryBookDefaultNode)
                                                        {

                                                            foreach (XmlNode finalNode in aCategoryBookDefaultVORowNode)
                                                            {


                                                                if (finalNode.Name == "AssetNumber")
                                                                {
                                                                    //Remove Null from list
                                                                    finalNode.Attributes.RemoveAll();
                                                                    //Replacement Code Goes here
                                                                    finalNode.InnerText = lineItem[1].ToString();
                                                                    Debug.WriteLine(finalNode.InnerText);






                                                                }
                                                            }

                                                        }
                                                    }
                                                }





                                            }






                                        }


                                    }

                                }
                            }



                        }


                    }



                    //get Comparitor (categoryCombination)
                    //XmlNode compareNode = aNodes.ChildNodes.Item(42);



                }
            }
            //myeditableXML.Save(editableFilePath);
            myEditable.Save(@"C:\Users\local act\Documents\myXMLfile.xml");






        }

        private void On_Runable_Clikc(object sender, RoutedEventArgs e)
        {
            //Create Editor
            var myEditor = getEditorxmls(@"C:\\Users\\local act\\Documents\\Category.xlsx");

            //Get Editable
            var myEditable = getEditableDocument(@"C:\\Users\\local act\\OneDrive\\Desktop\\FA_ASSET_CATEGORY.xml");

            //Edit and Save File
            EditDocument(myEditable, myEditor, @"C:\\Users\\local act\\OneDrive\\Desktop\\FA_ASSET_CATEGORY.xml");

            //Update to completed in Page





            //Get document atht will be INJECTED INTO
            //EditDocument("C:\\Users\\local act\\Desktop\\FA_ASSET_CATEGORY.xml");
        }

        private void CloseAppCLick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {

            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }

        }

        //
        private bool IsMaximized = false;
        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2)
            {
                if (IsMaximized)
                {
                    this.WindowState = WindowState.Normal;
                    this.Width = 1080;
                    this.Height = 720;

                    IsMaximized = false;
                }
                else
                {
                    this.WindowState = WindowState.Maximized;

                    IsMaximized = true;
                }
            }

        }

        private void yellowArrowAnimation()
        {
            while (true)
            {
                Seven.Foreground = new SolidColorBrush(Colors.Black);
                One.Foreground = new SolidColorBrush(Colors.Yellow);
                System.Threading.Thread.Sleep(1000);
                One.Foreground = new SolidColorBrush(Colors.Black);
                Two.Foreground = new SolidColorBrush(Colors.Yellow);
                System.Threading.Thread.Sleep(1000);
                Two.Foreground = new SolidColorBrush(Colors.Black);
                Three.Foreground = new SolidColorBrush(Colors.Yellow);
                System.Threading.Thread.Sleep(1000);
                Three.Foreground = new SolidColorBrush(Colors.Black);
                Four.Foreground = new SolidColorBrush(Colors.Yellow);
                System.Threading.Thread.Sleep(1000);
                Four.Foreground = new SolidColorBrush(Colors.Black);
                Five.Foreground = new SolidColorBrush(Colors.Yellow);
                System.Threading.Thread.Sleep(1000);
                Five.Foreground = new SolidColorBrush(Colors.Black);
                Six.Foreground = new SolidColorBrush(Colors.Yellow);
                System.Threading.Thread.Sleep(1000);
                Six.Foreground = new SolidColorBrush(Colors.Black);
                Seven.Foreground = new SolidColorBrush(Colors.Yellow);
                System.Threading.Thread.Sleep(1000);
            }


        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog theDialog = new OpenFileDialog();
            theDialog.Title = "Open Text File";
            theDialog.Filter = "XLSX files|*.xlsx";
            theDialog.InitialDirectory = @"C:\";
            string myInjectionAbsolute;
            if (theDialog.ShowDialog() == true)
            {
                //Set File Name
                FileNameLeft.Text = theDialog.SafeFileName;
                //Set Path
                PathLeft.Text = theDialog.FileName;

                //Add Debug Message
                Debug.WriteLine("File Loaded: " + theDialog.FileName.ToString());
            }
            
            if (PathRight.Text != "MISSING" && PathLeft.Text != "MISSING")
            {
                //Activate Injection Button Message
                Debug.WriteLine("Awaiting Injection Task");

                //Inject Button Active
                InjectionButton.IsEnabled = true;
            }

        }

        private void UploadXMLFileButton(object sender, RoutedEventArgs e)
        {
            OpenFileDialog theDialog = new OpenFileDialog();
            theDialog.Title = "Open Text File";
            theDialog.Filter = "XML files|*.xml";
            theDialog.InitialDirectory = @"C:\";
            if (theDialog.ShowDialog() == true)
            {
                //Set File Name
                FileNameRight.Text = theDialog.SafeFileName;
                //Set Path
                PathRight.Text = theDialog.FileName;

                //Add Debug Message
                Debug.WriteLine("File Loaded: " + theDialog.FileName.ToString());
            }


            if (PathRight.Text != "MISSING" && PathLeft.Text != "MISSING")
            {
                //Activate Injection Button Message
                Debug.WriteLine("Awaiting Injection Task");

                //Inject Button Active
                InjectionButton.IsEnabled = true;
            }

        }
    }
}
