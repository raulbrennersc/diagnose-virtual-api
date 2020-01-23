using DiagnoseVirtual.Domain.Entities;
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using System.Text;
using System.IO;
using System.Linq;
using NTSGeometries = NetTopologySuite.Geometries;
using SharpKml.Base;
using SharpKml.Dom;
using NetTopologySuite.IO;
using NetTopologySuite.Geometries;
using System.IO.Compression;
using NetTopologySuite.IO.ShapeFile.Extended;

namespace DiagnoseVirtual.Application.Helpers
{
    public static class GeoFileHelper
    {

        public static List<NTSGeometries.Geometry> ReadFile(IFormFile file, string path)
        {
            var uploads = Path.Combine(path, "uploads", DateTime.Now.Ticks.ToString());
            var fileExt = file.FileName.Split('.').Last();
            var geometrias = new List<NTSGeometries.Geometry>();
            switch (fileExt)
            {
                case "kml":
                    geometrias = ReadKml(file);
                    break;
                case "wkt":
                    geometrias = ReadWkt(file);
                    break;
                case "zip":
                case "shp":
                    geometrias = ReadShapefile(file, uploads);
                    break;
            }
            return geometrias;
        }

        public static List<NTSGeometries.Geometry> ReadKml(IFormFile file)
        {
            var parser = new Parser();
            parser.Parse(file.OpenReadStream());
            var a = (Kml)parser.Root;
            var document = (Document)a.Feature;
            var features = document.Features;

            var polygons = new List<SharpKml.Dom.LinearRing>();
            foreach (var feature in features)
            {
                var polygon = ((SharpKml.Dom.Polygon)((Placemark)feature).Geometry).OuterBoundary.LinearRing;
                polygons.Add(polygon);
            }

            var factory = NTSGeometries.Geometry.DefaultFactory;
            var NTSPolygons = polygons
                .Select(g => factory.CreatePolygon(g.Coordinates.Select(c => new Coordinate(c.Latitude, c.Longitude)).ToArray())).ToList();
            var geometrias = NTSPolygons.Select(p => (NTSGeometries.Geometry)factory.CreateGeometry(p)).ToList();
            return geometrias;
        }

        public static List<NTSGeometries.Geometry> ReadWkt(IFormFile file)
        {
            var factory = NTSGeometries.Geometry.DefaultFactory;
            var wktReader = new WKTReader(factory);
            var wktFileReader = new WKTFileReader(file.OpenReadStream(), wktReader);
            var polygons = wktFileReader.Read();

            var NTSPolygons = polygons
                .Select(g => factory.CreatePolygon(g.Coordinates.Select(c => new Coordinate(c.X, c.Y)).ToArray())).ToList();
            var geometrias = NTSPolygons.Select(p => factory.CreateGeometry(p)).ToList();
            return geometrias;
        }

        public static List<NTSGeometries.Geometry> ReadShapefile(IFormFile file, string uploads)
        {
            Directory.CreateDirectory(uploads);
            var filePath = Path.Combine(uploads, file.FileName);
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                file.CopyTo(fileStream);
            }

            var extrair = file.FileName.Split('.').Last() == "zip";

            if (extrair)
            {
                var finalDirectory = Path.Combine(uploads, file.FileName.Replace(".zip", ""));
                ZipFile.ExtractToDirectory(filePath, finalDirectory);
                var extractedFiles = Directory.GetFiles(finalDirectory + "\\layers");
                filePath = extractedFiles.FirstOrDefault(f => f.EndsWith(".shp"));
            }

            var geometrias = new List<NTSGeometries.Geometry>();

            using (var reader = new ShapeDataReader(filePath))
            {
                var mbr = reader.ShapefileBounds;
                var polygons = reader.ReadByMBRFilter(mbr).Select(r => r.Geometry);

                var factory = NTSGeometries.Geometry.DefaultFactory;
                var NTSPolygons = polygons
                    .Select(g => factory.CreatePolygon(g.Coordinates.Select(c => new Coordinate(c.X, c.Y)).ToArray())).ToList();
                geometrias = NTSPolygons.Select(p => factory.CreateGeometry(p)).ToList();
            }

            Directory.Delete(uploads, true);

            return geometrias;
        }
    }
}
