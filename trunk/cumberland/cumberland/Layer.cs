// Layer.cs
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

using Cumberland.Data;

namespace Cumberland
{
	public enum ThemeType
	{
		None,
		Unique,
		NumericRange
	}
	
	public class Layer
	{
		#region properties
		
		public IFeatureSource Data {
			get {
				return data;
			}
			set {
				data = value;
			}
		}

		public string Projection {
			get {
				return projection;
			}
			set {
				projection = value;
			}
		}

		public string Id {
			get {
				return id;
			}
			set {
				id = value;
			}
		}

		public List<Style> Styles {
			get {
				return styles;
			}
		}

		public ThemeType Theme {
			get {
				return themeType;
			}
			set {
				themeType = value;
			}
		}

		public string ThemeField {
			get {
				return themeField;
			}
			set {
				themeField = value;
			}
		}

		public string LabelField {
			get {
				return labelField;
			}
			set {
				labelField = value;
			}
		}

		IFeatureSource data;
		
		string projection = null;
		
		string id;
		
		List<Style> styles = new List<Style>();

		ThemeType themeType = ThemeType.None;
		
		string themeField = null;

		string labelField = null;
		
		#endregion

		#region public methods
		
		public Style GetRangeStyleForFeature(string fieldValue)
		{
			double val;
			if (!double.TryParse(fieldValue, out val))
		    {
				return null;
			}
			
			foreach (Style s in Styles)
			{
				if (val <= s.MaxRangeThemeValue &&
				    val >= s.MinRangeThemeValue)
				{
					return s;
				}
			}
			
			return null;
		}

		public Style GetUniqueStyleForFeature(string fieldValue)
		{
			foreach (Style s in Styles)
			{
				if (s.UniqueThemeValue == fieldValue)
				{
					return s;
				}
			}
			
			return null;
		}

		public Style GetStyleForFeature(string fieldValue)
		{
			if (Styles.Count == 0) return null;
			
			if (Theme == ThemeType.NumericRange)
			{
				return GetRangeStyleForFeature(fieldValue);
			}
			else if (Theme == ThemeType.Unique)
			{
				return GetUniqueStyleForFeature(fieldValue);
			}
			else
			{
				return Styles[0];
			}
		}

		#endregion
	}
}
