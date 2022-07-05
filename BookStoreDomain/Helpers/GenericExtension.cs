using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStoreDomain.Helpers
{
    public static class GenericExtension
    {
        public static void MapSourceToDestination<TSource, TDestination>(this TSource source, TDestination destination)

     where TSource : class, new()

     where TDestination : class, new()
        {

            if (source == null || destination == null) return;

            var sourceProperties = source.GetType().GetProperties().ToList();
            var destinationProperties = destination.GetType().GetProperties().ToList();
            foreach (var sourceProperty in sourceProperties)
            {
                var destinationProperty = destinationProperties.Find(item => item.Name == sourceProperty.Name);
                if (destinationProperty == null) continue;
                try
                {
                    destinationProperty.SetValue(destination, sourceProperty.GetValue(source, null), null);
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
        }
        public static TDestination MapProperties<TDestination>(this object source)
            where TDestination : class, new()
        {
            var destination = Activator.CreateInstance<TDestination>();
            MapSourceToDestination(source, destination);
            return destination;
        }

    }
}
