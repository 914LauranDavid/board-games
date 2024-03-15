using board_games.src.FeatureToggle;
using System.Diagnostics;

namespace board_games
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Delete before merge
            FeatureToggle features = new FeatureToggle();
            features.AddFeature("feature1");
            features.AddFeature("feature2");
            Debug.Assert(features.GetLength() == 2);
            try
            {
                features.AddFeature(null);
            }
            catch (ArgumentNullException e)
            {
                Console.WriteLine("Caught exception: {0}", e.Message);
            }
            try
            {
                features.AddFeature("");
            }
            catch (ArgumentNullException e)
            {
                Console.WriteLine("Caught exception: {0}", e.Message);
            }
            Debug.Assert(features.GetLength() == 2);

            Debug.Assert(features.GetFeatureState("feature1") == false);
            Debug.Assert(features.GetFeatureState("feature2") == false);

            features.EnableFeature("feature1");

            Debug.Assert(features.GetFeatureState("feature1") == true);

            features.DisableFeature("feature1");

            Debug.Assert(features.GetFeatureState("feature1") == false);
        }
    }
}
