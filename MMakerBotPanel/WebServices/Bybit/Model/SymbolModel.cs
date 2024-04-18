namespace MMakerBotPanel.WebServices.Bybit.Model
{
    using MMakerBotPanel.Models;

    public class SymbolModel
    {
        public SymbolModel()
        {
            genericResult = new GenericResult();
            GetSymbolDatasModels = new GetSymbolData();
        }
        public GenericResult genericResult;

        public GetSymbolData GetSymbolDatasModels;
    }
}