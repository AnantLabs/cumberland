// MapDrawer.cs
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
using System.Collections.Generic;
using System.Drawing;
using Cumberland;
using Cumberland.Data;


namespace Cumberland.Drawing
{
	public class MapDrawer : IMapDrawer
	{
		
		public MapDrawer()
		{
		}

		public Bitmap Draw (Map map)
		{
			ProjFourWrapper dst = null;
			
			Bitmap b = null;
			Graphics g = null;
			
			try
			{
				b = new Bitmap(map.Width, map.Height);
				g = Graphics.FromImage(b);
				
				// instantiate map projection
				if (!string.IsNullOrEmpty(map.Projection))
				{
					dst = new ProjFourWrapper(map.Projection);
				}
								
				int idx = -1;
				foreach (Layer layer in map.Layers)
				{
					idx++;
					
					if (layer.Data == null)
					{
						continue;
					}
					
					// query our data
					List<Feature> features = layer.Data.GetFeatures(map.Extents);
					
					if (features.Count == 0)
					{
						continue;
					}
					
					ProjFourWrapper src = null;
					
					try
					{
						// instantiate layer projection
						bool reproject = false;
						if (dst != null && !string.IsNullOrEmpty(layer.Projection))
						{
							src = new ProjFourWrapper(layer.Projection);
							reproject = true;
						}
					
						if (layer.Data.SourceFeatureType == FeatureType.Point)
					    {	

		#region handle point rendering
							
//							Gl.glPointSize(layer.PointSize);
//						    Gl.glBegin(Gl.GL_POINTS);
//							
//							Gl.glColor4ub(layer.FillColor.R, layer.FillColor.G, layer.FillColor.B, layer.FillColor.A);
//						
//							for (int ii=0; ii < features.Count; ii++)
//							{
//								Point p = features[ii] as Point;
//									
//								if (reproject)
//								{
//									p = src.Transform(dst, p);
//								}
//								
//								//g.DrawRectangle(new Pen(layer.FillColor), 
//									
////								Gl.glVertex2d(p.X, p.Y);			
//							}

	
		#endregion
						}
						else if (layer.Data.SourceFeatureType == FeatureType.Polyline)
						{
		#region Handle line rendering
							
							if (layer.LineStyle == LineStyle.None)
							{
								continue;
							}
							
//							// enable anti-aliasing					
//						    Gl.glEnable(Gl.GL_LINE_SMOOTH);
//							Gl.glEnable(Gl.GL_BLEND);
//							
//							if (layer.LineStyle != LineStyle.Solid)
//							{
//								Gl.glEnable(Gl.GL_LINE_STIPPLE);
//								Gl.glLineStipple(1, (short) layer.LineStyle);
//							}
//							
//							// to get the anti-aliased line to properly blend with the background color,
//							// we tell OpenGL to:
//							// multiply the R,G,B values of the line times it's alpha value 
//							// this alters the color by it's opacity
//							// next, we multiply the R,G,B values of the background times 1-the line's alpha value
//							// this applies the remaining alpha value to the background color
//							// (i.e. if the line is fully opaque, we want no interaction from the background color)
//							// the alpha values are preserved as is
//							// lastly, the two colors are merged (added together)
//							// http://glprogramming.com/red/chapter06.html#name1
//							Gl.glBlendFuncSeparate(Gl.GL_SRC_ALPHA, Gl.GL_ONE_MINUS_SRC_ALPHA, Gl.GL_ONE, Gl.GL_ONE);
//							
//							Gl.glLineWidth(layer.LineWidth);
//						
//						    for (int ii=0; ii < features.Count; ii++)
//							{
//								PolyLine pol = (PolyLine) features[ii];
//								for (int jj=0; jj < pol.Lines.Count; jj++)
//								{
//									Line r = pol.Lines[jj] as Line;
//								
//								    Gl.glBegin(Gl.GL_LINE_STRIP);
//
//									Gl.glColor4ub(layer.LineColor.R, layer.LineColor.G, layer.LineColor.B, layer.LineColor.A);
//									
//								    for (int kk = 0; kk < r.Points.Count; kk++)
//									{	
//										Point p = r.Points[kk];
//										
//										if (reproject)
//										{
//											p = src.Transform(dst, p);
//										}
//										
//										Gl.glVertex2d(p.X, p.Y);
//									}
//								
//								    Gl.glEnd();
//								}
//							}
//						
//							Gl.glDisable(Gl.GL_BLEND);
//						    Gl.glDisable(Gl.GL_LINE_SMOOTH);
//							
//							if (layer.LineStyle != LineStyle.Solid)
//							{
//								Gl.glDisable(Gl.GL_LINE_STIPPLE);
//							}
							
		#endregion
						}	
						else if (layer.Data.SourceFeatureType == FeatureType.Polygon)
						{
//							IntPtr tess = IntPtr.Zero;
//							
//							// we need to hold references to these delegates because we are going unmanaged
//							//Glu.TessBeginCallback tessBegin = new Tao.OpenGl.Glu.TessBeginCallback(Gl.glBegin);
//							Glu.TessBeginCallback tessBegin = new Tao.OpenGl.Glu.TessBeginCallback(TessBeginHandler);
//							Glu.TessEndCallback tessEnd = new Tao.OpenGl.Glu.TessEndCallback(TessEndHandler);
//							//Glu.TessVertexCallback tessVert = new Tao.OpenGl.Glu.TessVertexCallback(Gl.glVertex3dv);
//							//Glu.TessVertexCallback tessVert = new Tao.OpenGl.Glu.TessVertexCallback(TessVertexHandler);
//							GluMethods.TessVertexCallback1 tessVert = new Cumberland.GluWrap.GluMethods.TessVertexCallback1(TessVertexHandler);
//							//Glu.TessVertexDataCallback tessVert = new Tao.OpenGl.Glu.TessVertexDataCallback(TessVertexHandler);
//							//GluMethods.TessVertexDataCallback1 tessVert = new GluWrap.GluMethods.TessVertexDataCallback1(TessVertexHandler);
//							Glu.TessErrorCallback tessErr = new Glu.TessErrorCallback(TessErrorHandler);
//									
//							try
//							{
//#region configure glu tess
//								
//								// OpenGL can directly display only simple convex polygons. 
//								// A polygon is simple if the edges intersect only at vertices, 
//								// there are no duplicate vertices, and exactly two edges meet at any vertex. 
//								// If your application requires the display of concave polygons, 
//								// polygons containing holes, or polygons with intersecting edges, 
//								// those polygons must first be subdivided into simple convex polygons 
//								// before they can be displayed. Such subdivision is called tessellation, 
//								// and the GLU provides a collection of routines that perform tessellation.
//								// --red book, chp 11
//								
//								tess = GluMethods.gluNewTess();
//										
//								GluMethods.gluTessCallback(tess, Glu.GLU_TESS_BEGIN, tessBegin);
//								GluMethods.gluTessCallback(tess, Glu.GLU_TESS_END, tessEnd);
//								GluMethods.gluTessCallback(tess, Glu.GLU_TESS_VERTEX, tessVert);
//								//GluMethods.gluTessCallback(tess, Glu.GLU_TESS_VERTEX_DATA, tessVert);
//								GluMethods.gluTessCallback(tess, Glu.GLU_TESS_ERROR, tessErr);
//								
//								// For a single contour, the winding number of a point is the signed number of 
//								// revolutions we make around that point while traveling once around the contour 
//								// (where a counterclockwise revolution is positive and a clockwise revolution is negative). 
//								// When there are several contours, the individual winding numbers are summed. 
//								// This procedure associates a signed integer value with each point in the plane. 
//								// Note that the winding number is the same for all points in a single region.
//								
//								// exterior are clockwise, holes are counter, so we use odd
//								// FIXME: not working...
//								GluMethods.gluTessProperty(tess, Glu.GLU_TESS_WINDING_RULE, Glu.GLU_TESS_WINDING_ODD);
//
//#endregion
//								
//								for (int ii=0; ii < features.Count; ii++)
//								{
//									Polygon po = features[ii] as Polygon;
//
//#region tessalate and render polygon fill
//									
//									GluMethods.gluTessBeginPolygon(tess, IntPtr.Zero);
//									
//									// tess callbacks do not occur until gluTessEndPolygon	
//									// we must pin our vertexes because agressive garbage collectors (i.e. MS.NET)
//									// will move this around and corrupt the pointers GLU has
//									// http://blogs.msdn.com/clyon/archive/2004/09/17/230985.aspx
//									// TODO: can this be optimized?
//									List<GCHandle> handles = new List<GCHandle>();
//									
//									for (int jj = 0; jj < po.Rings.Count; jj++)
//								    {
//										Ring r = po.Rings[jj];
//										
//										// another exterior hole is a new polygon
//										// (for simplicity's sake, I am going to assume that a hole is associated with the last ring)
//										if (!r.IsClockwise)
//										{
//											continue;
//										}
//										else if (jj > 0)
//										{
//											// end the last exterior polygon
//											GluMethods.gluTessEndPolygon(tess);
//											
//											// clear out handles to this polygon's coords
//											FreeAndClearHandles(handles);
//											
//											// start a new exterior polygon
//											GluMethods.gluTessBeginPolygon(tess, IntPtr.Zero);
//										}          
//										
//										// FIXME: not working.  maybe need to use multisampling
////										if (layer.LineStyle == LineStyle.None)
////										{
////											Gl.glEnable(Gl.GL_POLYGON_SMOOTH);
////											Gl.glEnable(Gl.GL_BLEND);
////											Gl.glBlendFuncSeparate(Gl.GL_SRC_ALPHA, Gl.GL_ONE_MINUS_SRC_ALPHA, Gl.GL_ONE, Gl.GL_ONE);
////										}
//										
//										// TODO: use display lists for interactive viewer
//										//int tl = Gl.glGenLists(1);
//									    
//									    Gl.glColor4ub(layer.FillColor.R, layer.FillColor.G, layer.FillColor.B, layer.FillColor.A);
//									
//										//Gl.glNewList(tl, Gl.GL_COMPILE);
//										
//										GluMethods.gluTessBeginContour(tess);
//										
//										for (int kk = 0; kk < r.Points.Count; kk++)
//										{
//											Point p = r.Points[kk];
//											
//											if (reproject)
//											{
//												p = src.Transform(dst, p);
//											}
//											
//											double[] coord = new double[] {p.X, p.Y, 0d};
//											
//											// get a handle that is pinned (i.e. will not move)
//											GCHandle gch = GCHandle.Alloc(coord, GCHandleType.Pinned);
//											handles.Add(gch);
//
//											// now we can pass the actual pointer to the array
//											GluMethods.gluTessVertex(tess, gch.AddrOfPinnedObject(), gch.AddrOfPinnedObject());
//										}	
//										
//										GluMethods.gluTessEndContour(tess);
//
////										if (layer.LineStyle == LineStyle.None)
////										{
////											Gl.glDisable(Gl.GL_BLEND);
////											Gl.glDisable(Gl.GL_POLYGON_SMOOTH);
////										}
//	
//										//Gl.glEndList();
//										
//		#endregion
//								    }
//									
//									GluMethods.gluTessEndPolygon(tess);
//
//									// clear out these last handles
//									FreeAndClearHandles(handles);
//#region draw polygon outline
//			
//									if (layer.LineStyle != LineStyle.None)
//									{
//									
//										for (int jj = 0; jj < po.Rings.Count; jj++)
//									    {
//											Ring r = po.Rings[jj];
//									
//											if (layer.LineStyle != LineStyle.Solid)
//											{
//												Gl.glEnable(Gl.GL_LINE_STIPPLE);
//												Gl.glLineStipple(1, (short) layer.LineStyle);
//											}
//											
//											Gl.glEnable(Gl.GL_LINE_SMOOTH);
//											Gl.glEnable(Gl.GL_BLEND);
//											
//											//Gl.glBlendFunc(Gl.GL_SRC_ALPHA, Gl.GL_ONE_MINUS_SRC_ALPHA);
//											//Gl.glBlendFunc(Gl.GL_SRC_ALPHA_SATURATE, Gl.GL_ONE_MINUS_SRC_ALPHA);
//											Gl.glBlendFuncSeparate(Gl.GL_SRC_ALPHA, Gl.GL_ONE_MINUS_SRC_ALPHA, Gl.GL_ONE, Gl.GL_ONE);
//											
//											Gl.glLineWidth(layer.LineWidth);
//											
//										    Gl.glBegin(Gl.GL_LINE_STRIP);
//											
//										    Gl.glColor4ub(layer.LineColor.R, layer.LineColor.G, layer.LineColor.B, layer.LineColor.A);
//												
//											for (int kk = 0; kk < r.Points.Count; kk++)
//											{	
//												Point p = r.Points[kk];
//												
//												if (reproject)
//												{
//													p = src.Transform(dst, p);
//												}
//												
//												Gl.glVertex2d(p.X, p.Y);
//											}							
//				
//				 							Gl.glEnd();
//											Gl.glDisable(Gl.GL_BLEND);
//											Gl.glDisable(Gl.GL_LINE_SMOOTH);
//											
//											if (layer.LineStyle != LineStyle.Solid)
//											{
//												Gl.glDisable(Gl.GL_LINE_STIPPLE);
//											}								
//										}
//									}
//#endregion
//								}
//							}
//							finally
//							{
//								if (tess != IntPtr.Zero) 
//								{
//									GluMethods.gluDeleteTess(tess);
//									
//									// hack: it seems neccessary on Mono to call this so as the pointers to these
//									// delegates stay alive in unmanaged land
//									GC.KeepAlive(tessBegin);
//									GC.KeepAlive(tessVert);
//									GC.KeepAlive(tessErr);
//									GC.KeepAlive(tessEnd);								
//								}
//							}
						}
					}
					finally
					{
						// dispose of proj4
						if (src != null)
						{
							src.Dispose();
						}
					}
					
				}
				
				return b;
			}
			finally
			{
				// dispose of proj4
				if (dst != null)
				{
					dst.Dispose();
				}
				
				if (g != null)
				{
					g.Dispose();
				}
			}
		}
	}
}