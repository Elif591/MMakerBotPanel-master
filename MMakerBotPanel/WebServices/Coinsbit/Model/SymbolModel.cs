namespace MMakerBotPanel.WebServices.Coinsbit.Model
{
    using MMakerBotPanel.Models;

    public class SymbolModel
    {
        public SymbolModel()
        {
            genericResult = new GenericResult();
            CoinsbitSymbolDataModels = new CoinsbitSymbolData();

        }
        public GenericResult genericResult;
        public CoinsbitSymbolData CoinsbitSymbolDataModels;
    }
}