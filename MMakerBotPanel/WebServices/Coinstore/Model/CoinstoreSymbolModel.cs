namespace MMakerBotPanel.WebServices.Coinstore.Model
{
    using MMakerBotPanel.Models;

    public class CoinstoreSymbolModel
    {
        public CoinstoreSymbolModel()
        {
            genericResult = new GenericResult();
            CoinstoreSymbolDataModels = new CoinstoreSymbolDataModel();
        }
        public GenericResult genericResult;
        public CoinstoreSymbolDataModel CoinstoreSymbolDataModels;
    }
}