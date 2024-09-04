using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using DatNguyenTool;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace NDT_RevitAPI
{
    [Transaction(TransactionMode.Manual)]
    internal class CreatePrintSetCmd : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {

            UIDocument uidoc = commandData.Application.ActiveUIDocument;
            Document doc = uidoc.Document;

            //select sheets in project browser
            ICollection<ElementId> ids = uidoc.Selection.GetElementIds();

            var sheets = new List<ViewSheet>();
            foreach(ElementId id in ids)
            {
                Element ele = doc.GetElement(id);
                if (ele is ViewSheet vs) sheets.Add(vs);
            }

            PrintManager pM = doc.PrintManager;
            List<string> sheetSetNames = SheetUtils.GetViewSetName(doc);

            if (sheets.Count > 0)
            {

                var window = new AddNewNameView();
                window.ShowDialog();

                if (window.DialogResult == true)
                {
                    string setName = window.NewName;
                    if (!sheetSetNames.Contains(setName))
                    {
                        using (Transaction t = new Transaction(doc, "CreatePrintSet"))
                        {
                            t.Start();

                            ViewSet myViewSet = new ViewSet();
                            foreach (ViewSheet view in sheets)
                            {
                                myViewSet.Insert(view);
                            }

                            pM.PrintRange = PrintRange.Select;

                            ViewSheetSetting viewSheetSetting = pM.ViewSheetSetting;
                            viewSheetSetting.CurrentViewSheetSet.Views = myViewSet;
                            viewSheetSetting.SaveAs(setName);

                            t.Commit();
                        }
                        MessageBox.Show("Done!", "Message");
                    }
                    else
                    {
                        MessageBox.Show("Name is already in use.\nEnter a unique name.", "Message");
                    }
                }
            }
            else
            {
                MessageBox.Show("0 sheets selected!", "Message");
            }

            return Result.Succeeded;
        }
    }
}
