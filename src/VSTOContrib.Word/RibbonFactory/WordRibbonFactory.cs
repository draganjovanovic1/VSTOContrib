using System.Reflection;
using System.Runtime.InteropServices;
using Microsoft.Office.Interop.Word;
using Microsoft.Office.Tools;
using VSTOContrib.Core;
using VSTOContrib.Core.RibbonFactory.Interfaces;

namespace VSTOContrib.Word.RibbonFactory
{
    [ComVisible(true)]
    public class WordRibbonFactory : Core.RibbonFactory.RibbonFactory
    {
        WordViewProvider wordViewProvider;

        public WordRibbonFactory(
            AddInBase addinBase,
            params Assembly[] assemblies)
            : base(addinBase, UseIfEmpty(assemblies, Assembly.GetCallingAssembly()), new WordViewContextProvider(), null, WordRibbonType.WordDocument.GetEnumDescription())
        {
        }

        public WordRibbonFactory(
            AddInBase addinBase,
            IViewLocationStrategy viewLocationStrategy,
            params Assembly[] assemblies)
            : base(addinBase, UseIfEmpty(assemblies, Assembly.GetCallingAssembly()), new WordViewContextProvider(), viewLocationStrategy, WordRibbonType.WordDocument.GetEnumDescription())
        {
        }

        protected override void ShuttingDown()
        {
            wordViewProvider.Dispose();
        }

        protected override void InitialiseRibbonFactoryController(IRibbonFactoryController controller, object application)
        {
            wordViewProvider = new WordViewProvider((Application)application);
            controller.Initialise(wordViewProvider);
            wordViewProvider.RegisterOpenDocuments();
        }

        public override void Ribbon_Load(Microsoft.Office.Core.IRibbonUI ribbonUi)
        {
            //Word does not raise a new document event when we are starting up, and initialise is too soon
            if (wordViewProvider != null)
                wordViewProvider.RegisterOpenDocuments();
            base.Ribbon_Load(ribbonUi);
        }
    }
}