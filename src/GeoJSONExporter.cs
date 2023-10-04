using Elements;
using Elements.GeoJSON;
using Elements.Geometry;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace GeoJSONExporter
{
    public static class GeoJSONExporter
    {
        /// <summary>
        /// The GeoJSONExporter function.
        /// </summary>
        /// <param name="model">The input model.</param>
        /// <param name="input">The arguments to the execution.</param>
        /// <returns>A GeoJSONExporterOutputs instance containing computed results and the model with any new elements.</returns>
        public static GeoJSONExporterOutputs Execute(Dictionary<string, Model> inputModels, GeoJSONExporterInputs input)
        {
            var output = new GeoJSONExporterOutputs();
            var spz = inputModels["Space Planning Zones"].AllElementsOfType<SpaceBoundary>();
            var origin = inputModels["location"].AllElementsOfType<Origin>().First();
            var features = new List<Feature>();
            foreach (var space in spz)
            {
                var spaceBoundary = space.Boundary.Perimeter;
                var vertices = new List<Vector3>(spaceBoundary.Vertices);
                vertices.Add(vertices[0]);
                var coords = new[] { vertices.Select(v => Position.FromVectorMeters(origin.Position, v)).ToArray() };
                var geoPolygon = new Elements.GeoJSON.Polygon(coords);
                var properties = new Dictionary<string, object> {
                    {"Name", space.Name},
                    {"Program Type", space.ProgramType},
                    {"fill", space.Material.Color.ToHex()}
                    };
                var feature = new Feature(geoPolygon, properties);
                features.Add(feature);
            }
            var fc = new FeatureCollection(features);
            var json = JsonConvert.SerializeObject(fc);
            input.Download.SetExportTextContents(json);
            return output;
        }
    }
}