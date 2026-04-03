using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
namespace Azuxiren.MG.Drawing;
/// <summary> Global extensions for all drawing/graphics utilities</summary>
public static class DrawingExtensions
{
	/// <summary>Returns the squared distance from given two points</summary>
	/// <param name="lhs">Argument for the distance function</param>
	/// <param name="rhs">Argument for the distance function</param>
	/// <returns>The squared distance between two points</returns>
	public static int DistanceSquared(this Point lhs, Point rhs)
	{
		(lhs - rhs).Deconstruct(out int dx, out int dy);
		return (dx * dx) + (dy * dy);
	}

	/// <summary>
	/// Evalutes the dot product of two vectors, which
	/// is also equivalent to |lhs|.|rhs|.cos (theta)
	/// where theta is the angle between the vectors
	/// </summary>
	/// <param name="lhs">The left operand</param>
	/// <param name="rhs">The right operand</param>
	/// <returns>The value of dot product, also equivalent to |lhs|.|rhs|.sin (theta)</returns>
	public static int Dot(this Point lhs, Point rhs) => (lhs.X * rhs.X) + (lhs.Y * rhs.Y);

	/// <summary>
	/// Returns the 2d cross product of vectors, 
	/// also equivaluent to |lhs|.|rhs|.sin (theta)
	/// where theta is the angle between the vectors
	/// </summary>
	/// <param name="lhs">The left operand</param>
	/// <param name="rhs">The right operand</param>
	/// <returns>The value of cross product, also equivalent to |lhs|.|rhs|.sin (theta)</returns>
	public static int CrossProduct2d(this Point lhs, Point rhs) => (lhs.X * rhs.Y) - (lhs.Y * rhs.X);

	/// <summary>
	/// Returns the 2d cross product of vectors, 
	/// also equivaluent to |lhs|.|rhs|.sin theta
	/// where theta is the angle between the vectors
	/// </summary>
	/// <param name="lhs">The left operand</param>
	/// <param name="rhs">The right operand</param>
	/// <returns>The value of cross product, also equivalent to |lhs|.|rhs|.sin (theta)</returns>
	public static float CrossProduct2d(this Vector2 lhs, Vector2 rhs) => (lhs.X * rhs.Y) - (lhs.Y * rhs.X);

	/// <summary>Computes the angle between two vectors</summary>
	/// <param name="lhs">The left operand</param>
	/// <param name="rhs">The right operand</param>
	/// <returns>The angle in radians between the two vectors</returns>
	public static float AngleBetween(this Vector2 lhs, Vector2 rhs)
		=> float.Atan2(
			lhs.CrossProduct2d(rhs),
			Vector2.Dot(lhs, rhs)
		);

	/// <summary>Computes the angle between two vectors</summary>
	/// <param name="lhs">The left operand</param>
	/// <param name="rhs">The right operand</param>
	/// <returns>The angle in radians between the two vectors</returns>
	public static float AngleBetween(this Point lhs, Point rhs)
		=> float.Atan2(
			lhs.CrossProduct2d(rhs),
			lhs.Dot(rhs)
		);

	/// <summary>Creates a rectangle of a given size placed such that its center lies at the given point</summary>
	/// <param name="center">Where the center of the rectangle should lie</param>
	/// <param name="size">The dimensions of the rectangle</param>
	public static Rectangle SetCenter(Point center, Point size)
		=> new
		(
			new Point
			(
				(center.X - size.X) / 2,
				(center.Y - size.Y) / 2
			),
			size
		);

	/// <summary>
	/// Prepares a rectangle such that it is fitted inside the container 
	/// rectangle such that it is scaled with its aspect ratio preserved,
	/// and it has the same center as the container rectangle
	/// </summary>
	/// <param name="size">The size of the contained Rectangle to scale and shift</param>
	/// <param name="rect">Rectangle taken as container/reference</param>
	/// <returns>The prepared rectangle</returns>
	public static Rectangle SetCenterScaled(Point size, Rectangle rect)
	{
		if (size.X <= 0 || size.Y <= 0) throw new ArgumentException(
			"Obtained invalid dimensions of size",
			paramName: nameof(size)
		);
		if (rect.Width <= 0 || rect.Height <= 0) throw new ArgumentException(
			"Obtained Rectangle of invalid dimensions",
			paramName: nameof(rect)
		);
		float scale = float.Min(
			rect.Width / (float)size.X,
			rect.Height / (float)size.Y
		);
		size.X = (int)(scale * size.X);
		size.Y = (int)(scale * size.Y);
		return SetCenter(rect.Center, size);
	}

	/// <summary>
	/// Fits a given number of rectangles in the LargeRectangle provided as i
	/// nput such that all inner rectangles are of the same width and height
	/// </summary>
	/// <param name="largeRectangle">The rectangle to fit other rectangles at</param>
	/// <param name="toFit">The number of small rectangles to fit</param>
	/// <param name="offset">The cleaerance(in px) between each rectangle</param>
	/// <param name="vertical">
	/// If true, the rectangles are fitted vertically. Otherwise 
	/// they are fitted horizontally
	/// </param>
	/// <returns>An array of rectangles that fit the area</returns>
	public static Rectangle[] FitRectangle(this Rectangle largeRectangle, byte toFit, uint offset = 0, bool vertical = false)
	{
		if (toFit == 0) throw new ArgumentException("Invalid box count");
		else if (toFit == 1) return [largeRectangle];
		int width, height;
		if (vertical)
		{
			width = largeRectangle.Width;
			height = (int)((largeRectangle.Height - ((toFit - 1) * offset)) / toFit);
		}
		else
		{
			height = largeRectangle.Height;
			width = (int)((largeRectangle.Width - ((toFit - 1) * offset)) / toFit);
		}
		if (height <= 0 || width <= 0)
			throw new ArgumentException(
			"Not possible to fit these many rectangles with the given LargeRectangle and offset"
		);
		Rectangle[] rectangles = new Rectangle[toFit];
		int x = largeRectangle.X, y = largeRectangle.Y;
		for (byte i = 0; i < toFit; i++)
		{
			rectangles[i] = new Rectangle(x, y, width, height);
			if (vertical) y += height + (int)offset;
			else x += width + (int)offset;
		}
		return rectangles;
	}

	/// <summary>
	/// Returns a matrix of Rectangles fitted as per requirements
	/// </summary>
	/// <param name="largeRectangle">The area to divide into</param>
	/// <param name="rectsInRow">The number of rectangles desired in a single row</param>
	/// <param name="xOffset">The offset distance between each rectangle in a single row</param>
	/// <param name="rectsInCollumn">The number of rectangles in a single collumn</param>
	/// <param name="yOffset">The offset distance between eacj rectangle in a single column</param>
	/// <returns>A grid of rectangles that fit the area</returns>
	public static Rectangle[,] FitRectangle(
		this Rectangle largeRectangle,
		byte rectsInRow,
		uint xOffset,
		byte rectsInCollumn,
		uint yOffset
	)
	{
		if (rectsInCollumn == 0 || rectsInRow == 0) throw new ArgumentException("Invalid box count");
		int width = (int)((largeRectangle.Height - ((rectsInRow - 1) * xOffset)) / rectsInRow);
		int height = (int)((largeRectangle.Width - ((rectsInCollumn - 1) * yOffset)) / rectsInCollumn);
		var rects = new Rectangle[rectsInRow, rectsInCollumn];
		int x, y = largeRectangle.Y;
		for (int i = 0; i < rectsInRow; i++)
		{
			x = largeRectangle.X;
			for (int j = 0; j < rectsInCollumn; j++)
			{
				rects[i, j] = new Rectangle(x, y, width, height);
				x += width + (int)xOffset;
			}
			y += height + (int)yOffset;
		}
		return rects;
	}

	/// <summary>
	/// Fits rectangles into a single Large rectangle according to given ratios and offset
	/// </summary>
	/// <param name="largeRectangle">The large Rectangle to be divided</param>
	/// <param name="ratios">The ratio of width/height of each rectangle</param>
	/// <param name="offset">The distance between each rectangle</param>
	/// <param name="vertical">If true, the large rectangle is divided vertically, otherwise horizontally</param>
	/// <returns>An array of rectangles that fit the area</returns>
	public static Rectangle[] FitRectangle(this Rectangle largeRectangle, byte[] ratios, uint offset, bool vertical = false)
	{
		var len = ratios.Length;
		if (len == 0)
			throw new ArgumentException(
				"There should be at least one value in the list of ratios",
				nameof(ratios)
			);
		else if (len == 1) return [largeRectangle];
		int sum = 0, i, x = largeRectangle.X, y = largeRectangle.Y, width, height;
		foreach (var ratio in ratios)
		{
			if (ratio == 0)
				throw new ArgumentException(
					"Invalid value of ratio as 0",
					paramName: nameof(ratios)
				);
			sum += ratio;
		}
		if (vertical)
		{
			width = largeRectangle.Width;
			height = (int)(largeRectangle.Height - ((len - 1) * offset)) / sum;
		}
		else
		{
			height = largeRectangle.Height;
			width = (int)(largeRectangle.Width - ((len - 1) * offset)) / sum;
		}
		if (width <= 0 || height <= 0) throw new ArgumentException("Invalid dimensions for the menu", nameof(largeRectangle));
		Rectangle[] rectangles = new Rectangle[len];
		for (i = 0; i < len; i++)
		{
			if (vertical)
			{
				rectangles[i] = new Rectangle(x, y, width, height * ratios[i]);
				y += (height * ratios[i]) + (int)offset;
			}
			else
			{
				rectangles[i] = new Rectangle(x, y, width * ratios[i], height);
				x += (width * ratios[i]) + (int)offset;
			}
		}
		return rectangles;
	}

	/// <summary>Generates a Texture Image from the grid of Colors</summary>
	/// <param name="grid">The grid of colors to generate image from</param>
	/// <param name="device">The graphicsdevice reference to be used</param>
	/// <returns>The converted texture image</returns>
	public static Texture2D FromColorGrid(this Color[,] grid, in GraphicsDevice device)
	{
		int r = grid.GetLength(0), c = grid.GetLength(1);
		Texture2D tex = new(device, r, c);
		Color[] dest = new Color[grid.Length];
		for (int i = 0, k = 0; i < c; i++)
		{
			for (int j = 0; j < r; j++)
			{
				dest[k++] = grid[j, i];
			}
		}
		tex.SetData(dest);
		return tex;
	}

	/// <summary>Iterates over all points lying in the lines between the
	/// two points in argument, using Bresenham line drawing algorithm</summary>
	/// <param name="p0">The first input point</param>
	/// <param name="p1">The second input point</param>
	/// <returns>Enumeration of points lying in the line between the points</returns>
	public static IEnumerable<Point> GetPointsOnLine(Point p0, Point p1)
	{
		int x0 = p0.X, x1 = p1.X, y0 = p0.Y, y1 = p1.Y;
		bool dxy = Math.Abs(y1 - y0) > Math.Abs(x1 - x0);
		if (dxy) (x0, y0, x1, y1) = (y0, x0, y1, x1);
		if (x0 > x1) (x0, y0, x1, y1) = (x1, y1, x0, y0);
		int dx = x1 - x0;
		int dy = Math.Abs(y1 - y0);
		int decision = dx / 2;
		int inc = (y0 < y1) ? 1 : -1;
		int y = y0;
		for (int x = x0; x <= x1; x++)
		{
			yield return dxy ? (new(y, x)) : (new(x, y));
			decision -= dy;
			if (decision < 0)
			{
				y += inc;
				decision += dx;
			}
		}
		yield break;
	}
	/// <summary>Denotes the count of pixels attempted and drawn by drawing function</summary>
	public struct DrawResult
	{
		/// <summary>The number of pixels attempted to be drawn</summary>
		public uint Attempted;
		/// <summary>The number of pixels that were drawn successfully</summary>
		public uint Drawn;
		/// <summary>The number of pixels that were outside the bounds and could not be drawn</summary>
		public readonly uint Missed => Attempted - Drawn;
	}
	/// <summary>Draws a line drawing on the grid, between the given points</summary>
	/// <param name="grid">The grid to draw on</param>
	/// <param name="pt1">The start point</param>
	/// <param name="pt2">The end point</param>
	/// <param name="color">The color of the line</param>
	/// <returns>DrawResult instance that shows how many points were drawn successfully</returns>
	public static DrawResult DrawLine(this Color[,] grid, Point pt1, Point pt2, Color color)
	{
		int rows = grid.GetLength(0), cols = grid.GetLength(1);
		DrawResult result = new();
		foreach (var point in GetPointsOnLine(pt1, pt2))
		{
			result.Attempted++;
			if (point.X < 0 || point.X > rows || point.Y < 0 || point.Y >= cols)
				continue;
			grid[point.X, point.Y] = color;
			result.Drawn++;
		}
		return result;
	}
	/// <summary>
	/// Uses the bresenham circle drawing algorithm to iterate
	/// over all points of a circle at a given centre and radius.
	/// The order of the points is not continous. The algorithm actually 
	/// iterates one octet of the circle. For each corresponding octet,
	/// a corresponding point from all other octets are selected.
	/// <br/><br/>
	/// To be precise, if we imagine the circle starting from x=radius,y=0
	/// (i.e rightmost point) as angle 0, and move clockwise to angle 360,
	/// and we number the octets from here for each arc of 45 degrees as 
	/// octets 1 to 8, then the points are from the octets in the following
	/// order: [7, 2, 6, 3, 8, 1, 5, 4]
	/// <br/>
	/// This ensures that the series of points generated from this algorithm
	/// has the following property: for points generated by the algorithm as
	/// [p1, p2, p3, ... ] If any point at the odd index is (Cx+x, Cx+y) where
	/// Cx and Cy the coordinates of the center, the the next number(at even index)
	/// is guaranteed to be (Cx+x, Cx-y). This property is used for the filled
	/// color algorithm   
	/// </summary>
	/// <param name="center">Centre of the circle</param>
	/// <param name="radius">radius of the circle</param>
	/// <returns>Enumeration of all points on the circle</returns>
	public static IEnumerable<Point> GetPointsOnCircle(Point center, int radius)
	{
		if (radius <= 1) throw new ArgumentException(
			"Radius should be greater than 1",
			nameof(radius)
		);
		int x = 0, y = radius, d = 3 - (2 * radius);
		do
		{
			yield return new(center.X + x, center.Y + y);
			yield return new(center.X + x, center.Y - y);
			yield return new(center.X - x, center.Y + y);
			yield return new(center.X - x, center.Y - y);
			yield return new(center.X + y, center.Y + x);
			yield return new(center.X + y, center.Y - x);
			yield return new(center.X - y, center.Y + x);
			yield return new(center.X - y, center.Y - x);

			d = d > 0 ?
				d + (4 * (x - (--y))) + 10 :
				d + (4 * x) + 6;
		} while (y >= x++);
	}

	/// <summary>Attempts to draw a filled circle on the given grid at the given position</summary>
	/// <param name="grid">The grid on which the circle is to be drawn</param>
	/// <param name="circle">The parameters of the circle to draw</param>
	/// <param name="color">The fill color</param>
	/// <returns>DrawResult instance that shows how many points were drawn successfully</returns>
	public static DrawResult DrawCircleFilled(this Color[,] grid, IntCircle circle, Color color)
	{
		DrawResult result = new();
		int rows = grid.GetLength(0), cols = grid.GetLength(1);
		IEnumerable<Point> point_collection = GetPointsOnCircle(circle.Center, circle.Radius);
		do
		{
			Point[] pair = [.. point_collection.Take(2)];
			if (pair.Length < 2 || pair[0].X != pair[1].X || pair[0].Y < pair[1].Y) break;
			for (int id_i = pair[0].X, id_j = pair[0].Y; id_j >= pair[1].Y; id_j--)
			{
				result.Attempted++;
				if (id_i < 0 || id_i >= rows || id_j < 0 || id_j >= cols) continue;
				grid[id_i, id_j] = color;
				result.Drawn++;
			}
		} while (true);
		return result;
	}

	/// <summary>Attempts to draw a bordered circle on the given grid at the given position</summary>
	/// <param name="grid">The grid on which the circle is to be drawn</param>
	/// <param name="circle">The parameters of the circle to draw</param>
	/// <param name="color">The fill color</param>
	/// <param name="thick">The thickness of the circle</param>
	/// <returns>DrawResult instance that shows how many points were drawn successfully</returns>
	public static DrawResult DrawCircleBorder(this Color[,] grid, IntCircle circle, Color color, byte thick = 1)
	{
		DrawResult result = new();
		int rows = grid.GetLength(0), cols = grid.GetLength(1);
		for (int i = 0; i < thick; i++)
		{
			foreach (Point p in GetPointsOnCircle(circle.Center, circle.Radius - i))
			{
				result.Attempted++;
				if (p.X < 0 || p.X >= rows || p.Y < 0 || p.Y >= cols) continue;
				grid[p.X, p.Y] = color;
				result.Drawn++;
			}
		}
		return result;
	}

	/// <summary>Sets the angle in the bracket [-pi, pi]</summary>
	/// <param name="angle">The angle to set</param>
	/// <returns>The angle set between [-pi, pi]</returns>
	public static float AngleMod(float angle)
	{
		const float twoPi = float.Pi * 2;
		angle %= twoPi;
		if (angle > float.Pi)
			angle -= twoPi;
		else if (angle <= -float.Pi)
			angle += twoPi;
		return angle;
	}
}