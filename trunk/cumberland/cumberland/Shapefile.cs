// Shapefile.cs
//
// Copyright (c) 2008 Scott Ellington and Authors
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
//
//

using System;
using System.IO;
using System.Collections.Generic;

namespace Cumberland 
{
    public class Shapefile 
	{				
        public enum ShapeType
        {
            Null        = 0,
            Point       = 1,
            PolyLine    = 3,
            Polygon     = 5,
            MultiPoint  = 8,
            PointZ      = 11,
            PolyLineZ   = 13,
            PolygonZ    = 15,
            MultiPointZ = 18,
            PointM      = 21,
            PolyLineM   = 23,
            PolygonM    = 25,
            MultiPointM = 28,
            MultiPatch  = 31
        }
		
#region Vars
		
		// TODO: expose extents
        Point min;
        Point max;

		uint filelength;
		uint version;
		string filename;
		//spublic int stamp;
		
#endregion
		
#region Properties
	
		public ShapeType Shapetype {
			get {
				return shapetype;
			}
			set {
				shapetype = value;
			}
		}
		ShapeType shapetype = ShapeType.Null;
		
		public List<Feature> Features {
			get {
				return features;
			}
			set {
				features = value;
			}
		}
		public List<Feature> features = new List<Feature>();

#endregion

#region ctor
		
        public Shapefile(string fname)
        {
	        FileStream file;

			file = new FileStream(fname, FileMode.Open, FileAccess.Read); 
           	
			BinaryReader str = new BinaryReader(file);
            str.BaseStream.Seek(0, SeekOrigin.Begin); 
			
            ReadFileHeader(str);
			ReadShapeRecords(str);
			
            file.Close();
			
            filename = fname.Substring(fname.LastIndexOf('/')+1);
		}

		
#endregion
		
#region Helper methods		
		
        uint FlipEndian(uint iin)
        {
            byte[] temp = BitConverter.GetBytes(iin);
            Array.Reverse(temp);
            return BitConverter.ToUInt32(temp,0);
        }

        void ReadFileHeader(BinaryReader stream)
        {
            // grab the File Code
            uint filecode = FlipEndian(stream.ReadUInt32());

            if (filecode != 9994)
            {
				throw new InvalidDataException("This file does not appear to be a shapefile");
            }

            // the next 20 bytes are junk
            byte[] junk = new byte[20];
            stream.Read(junk,0,junk.Length);

            // grab the file length
            filelength = FlipEndian(stream.ReadUInt32());
            //Console.WriteLine("INFO: File Length (in 16-bit words) is " + filelength);

            // get version
            version = stream.ReadUInt32();
            //Console.WriteLine("INFO: Version is " + version);

            // get shape type
            shapetype = (ShapeType) stream.ReadUInt32();
            //Console.WriteLine("INFO: ShapeType is " + shapetype);

            // get extents
            double xmin, ymin, zmin, mmin, xmax, ymax, zmax, mmax;
            xmin = stream.ReadDouble();
            ymin = stream.ReadDouble();
            xmax = stream.ReadDouble();
            ymax = stream.ReadDouble();
            zmin = stream.ReadDouble();
            zmax = stream.ReadDouble();
            mmin = stream.ReadDouble();
            mmax = stream.ReadDouble();
            min = new Point(xmin, ymin, zmin, mmin);
            max = new Point(xmax, ymax, zmax, mmax);
            
            //Console.WriteLine("INFO: Extents: (" + min.X + "," + min.Y + "," + min.Z + 
            //       "," + min.M + ") (" + max.X + "," + max.Y + "," + max.Z + "," + max.M + ")");
			
        }

		void ReadShapeRecords(BinaryReader stream)
		{
		   	uint loc = 50;  // current position in file
		   
			while (loc < filelength)
			{
				uint recordNum = FlipEndian(stream.ReadUInt32());
				uint recordLen = FlipEndian(stream.ReadUInt32());
				uint recordShp = stream.ReadUInt32();
				
				//Console.WriteLine("INFO: Record # " + recordNum + " has length " + recordLen +
				//	  				" and type " + recordShp);  

				// Chop off 32 bit from our remaining record because we read the shape type
				uint dataleft = recordLen - 2;
				switch (recordShp)
				{
				   	case 0:
					   	// Null Object, nothing to read in
					   	//Console.WriteLine("INFO: Null Shape Found");
						break;
					case 1:
						// Read in Point object
						Point p = new Point(stream.ReadDouble(), stream.ReadDouble());
						p.Id = recordNum;
						features.Add(p);
						break;
					case 3:
						// Read in PolyLine object
					   	PolyLine po = GetPolyLine(stream, dataleft);
						po.Id = recordNum;
						features.Add(po);						
						break;
					case 5:
						// Read in Polygon object
					   	Polygon pol = GetPolygon(stream, dataleft);
						pol.Id = recordNum;
						features.Add(pol);						
						break;
					default:
						// Anything unsupported gets dumped
						//Console.WriteLine("INFO: Unsupported Shape Type");
						byte[] data = new byte[dataleft * 2];
						stream.Read(data, 0, data.Length);
						break;
				}
				
				// log distance we have traversed in file: record length + header size
				loc += recordLen + 4;
			}
		}
		
		Polygon GetPolygon(BinaryReader stream, uint dlen)
		{
			double xmin = stream.ReadDouble();
			double ymin = stream.ReadDouble();
			double xmax = stream.ReadDouble();
			double ymax = stream.ReadDouble();

			uint numParts = stream.ReadUInt32();
			uint numPoints = stream.ReadUInt32();

			uint[] parts = new uint[numParts];
			int ii;
			for (ii=0; ii < numParts; ii++)
				parts[ii] = stream.ReadUInt32();

			Ring[] rings = new Ring[numParts];
			for (ii=0; ii < rings.Length; ii++)
			   	rings[ii] = new Ring();
			
			Polygon po = new Polygon(xmin, ymin, xmax, ymax);

			ii=0;
			for (int jj=0; jj < numPoints; jj++)
			{
				if (ii < parts.Length-1)
				   	if (parts[ii+1] == jj)
					{
						po.Rings.Add(rings[ii]);
					   	ii++;
					}
				Point p = new Point(stream.ReadDouble(), stream.ReadDouble());
				rings[ii].Points.Add(p);				
			}
			po.Rings.Add(rings[ii]);
			
			return po;
		}
		
		PolyLine GetPolyLine(BinaryReader stream, uint dlen)
		{
			double xmin = stream.ReadDouble();
			double ymin = stream.ReadDouble();
			double xmax = stream.ReadDouble();
			double ymax = stream.ReadDouble();

			uint numParts = stream.ReadUInt32();
			uint numPoints = stream.ReadUInt32();

			uint[] parts = new uint[numParts];
			int ii;
			for (ii=0; ii < numParts; ii++)
				parts[ii] = stream.ReadUInt32();

			Line[] lines = new Line[numParts];
			for (ii=0; ii < lines.Length; ii++)
			   	lines[ii] = new Line();
			
			PolyLine po = new PolyLine(xmin, ymin, xmax, ymax);

			ii=0;
			for (int jj=0; jj < numPoints; jj++)
			{
				if (ii < parts.Length-1)
				   	if (parts[ii+1] == jj)
					{
						po.Lines.Add(lines[ii]);
					   	ii++;
					}
				Point p = new Point(stream.ReadDouble(), stream.ReadDouble());
				lines[ii].Points.Add(p);				
			}
			po.Lines.Add(lines[ii]);
			
			return po;
		}
		
#endregion
    }
}
