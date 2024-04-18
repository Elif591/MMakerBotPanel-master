namespace MMakerBotPanel.Business.Strategy.DexGrid
{
    using MMakerBotPanel.Business.Strategy.Interfaces;

    public class DexGridConcreateCreator : FacConcreteCreatorAbstract
    {

        private static DexGridConcreateCreator _DexGridConcreateCreator;

        private static readonly object _lock = new object();

        private DexGridConcreateCreator()
        {

        }

        public static FacConcreteCreatorAbstract GetDexGridConcreateCreator()
        {
            lock (_lock)
            {
                if (_DexGridConcreateCreator == null)
                {
                    _DexGridConcreateCreator = new DexGridConcreateCreator();
                }
                return _DexGridConcreateCreator;
            }
        }

        public override bool BotStartingMethods(object parameters)
        {
            return _DexGridConcreateCreator.BotStartingMethods(parameters);
        }

        public override bool BotStoppingMethods(FacIStrategy strategy)
        {
            return _DexGridConcreateCreator.BotStoppingMethods(strategy);
        }
    }
}