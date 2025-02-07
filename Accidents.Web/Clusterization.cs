using Accidents.Model;

namespace Accidents.Web
{
    public class Clusterization
    {
        public static List<Cluster> Clusterize(List<Accident> accidents, int dist, int count)
        {
            var clusters = new List<Cluster>();

            foreach (var accident in accidents)
            {
                var cluster = new Cluster();

                foreach (var accident2 in accidents.Where(a => a.Id != accident.Id))
                {
                    var distance = HaversineDistance(accident, accident2);
                    if (distance < dist)
                    {
                        cluster.Accidents.Add(accident2);
                    }
                }

                if (cluster.Accidents.Count >= count)
                {
                    clusters.Add(cluster);
                }
            }

            MergeClusters(clusters);

            return clusters.Where(c => !c.Removed).ToList();
        }

        private static void MergeClusters(List<Cluster> clusters)
        {
            foreach (var cluster in clusters)
            {
                if (cluster.Removed)
                {
                    continue;
                }

                foreach (var anotherCluster in clusters.Where(c => c != cluster))
                {
                    if (anotherCluster.Removed)
                    {
                        continue;
                    }

                    if (cluster.HasMatch(anotherCluster))
                    {
                        cluster.Merge(anotherCluster);
                        anotherCluster.Removed = true;
                    }
                }
            }
        }

        private static double HaversineDistance(Accident accident1, Accident accident2)
        {
            const double R = 6371; // Earth's radius in kilometers
            var dLat = ToRadians(accident2.Coordinates.Latitude.Value - accident1.Coordinates.Latitude.Value);
            var dLon = ToRadians(accident2.Coordinates.Longitude.Value - accident1.Coordinates.Longitude.Value);

            var a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                    Math.Cos(ToRadians(accident1.Coordinates.Latitude.Value)) * Math.Cos(ToRadians(accident2.Coordinates.Latitude.Value)) *
                    Math.Sin(dLon / 2) * Math.Sin(dLon / 2);
            var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            var distance = R * c;

            return distance * 1000; // meter
        }

        private static double ToRadians(double angle)
        {
            return Math.PI * angle / 180.0;
        }
    }

    public class Cluster
    {
        public List<Accident> Accidents { get; set; } = new();

        public void Merge(Cluster another)
        {
            this.Accidents.AddRange(another.Accidents);
            this.Accidents = Accidents.DistinctBy(i => i.Id).ToList();
        }

        public bool HasMatch(Cluster another)
        {
            return this.Accidents.Any(a => another.Accidents.Any(aa => a.Id == aa.Id));
        }

        public bool Removed { get; set; }
    }
}
