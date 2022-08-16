using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
namespace Azuxiren.MG
{
	/// <summary> Represents a side of a Rectangle</summary>
	[Flags]
	public enum RectangleSide : byte
	{
		/// <summary> No side of a Rectangle</summary>
		None = 0,
		/// <summary> Left side of a Rectangle</summary>
		Left = 1,
		/// <summary> Right side of a Rectangle</summary>
		Right = 2,
		/// <summary> Top of a Rectangle</summary>
		Top = 4,
		/// <summary> Bottom side of a Rectangle</summary>
		Bottom = 8
	}
	/// <summary> Global class for all utilities </summary>
	public static partial class Global
	{
		/// <summary>Shortcut to draw part of a image(Texture2D)</summary>
		/// <param name="tex">The Texture2D to draw</param>
		/// <param name="source">The portion of image to draw</param>
		/// <param name="dest">The position on screen of drawing</param>
		/// <param name="sb">The SpriteBatch Object</param>
		/// <param name="c">Tint color (by default white)</param>
		public static void Draw(this Texture2D tex, Rectangle source, Rectangle dest, SpriteBatch sb, Color? c = null)
			=> sb.Draw(tex, source, dest, c ?? Color.White);
		/// <summary>Shortcut to draw the image(Texture2D)</summary>
		/// <param name="tex">The image to draw</param>
		/// <param name="dest">The position on screen to draw</param>
		/// <param name="sb">The SpriteBatch Object</param>
		/// <param name="c">Tint color (by default White)</param>
		public static void Draw(this Texture2D tex, Rectangle dest, SpriteBatch sb, Color? c = null)
			=> sb.Draw(tex, dest, c ?? Color.White);
		/// <summary> Shortcut to draw an image on a Vector2D</summary>
		/// <param name="tex">The image to draw</param>
		/// <param name="dest">The Position to draw</param>
		/// <param name="sb">The SpriteBatch Object</param>
		/// <param name="c">Tint color (by default White)</param>
		public static void Draw(this Texture2D tex, Vector2 dest, SpriteBatch sb, Color? c = null)
			=> sb.Draw(tex, dest, c ?? Color.White);
		/// <summary>Writes a String on display at the provided Rectangle</summary>
		/// <param name="font">Spritefont object</param>
		/// <param name="sb">The SpriteBatch Object</param>
		/// <param name="dest">The Rectangle position to draw at</param>
		/// <param name="message">The content of the string</param>
		/// <param name="c">Tint color (by default White)</param>
		public static void Write(this SpriteFont font, SpriteBatch sb, Rectangle dest, string message, Color? c = null)
		{
			Vector2 size = font.MeasureString(message);
			float xScale = dest.Width / size.X;
			float yScale = dest.Height / size.Y;
			float scale = Math.Min(xScale, yScale);
			int strWidth = (int)Math.Round(size.X * scale);
			int strHeight = (int)Math.Round(size.Y * scale);
			Vector2 position = new()
			{
				X = ((dest.Width - strWidth) / 2) + dest.X,
				Y = ((dest.Height - strHeight) / 2) + dest.Y
			};
			float rotation = 0.0f;
			Vector2 spriteOrigin = new(0, 0);
			float spriteLayer = 0.0f; // all the way in the front
			SpriteEffects spriteEffects = SpriteEffects.None;
			Color cc = c ?? Color.White;// Draw the string to the sprite batch!
			sb.DrawString(font, message, position, cc, rotation, spriteOrigin, scale, spriteEffects, spriteLayer);
		}
		/// <summary>
		/// Sets rectangle a such that a and b share the same center
		/// </summary>
		/// <param name="a">The rectangle to shift</param>
		/// <param name="b">The rectangle whose center is to be taken reference of</param>
		public static void SetCenter(ref Rectangle a, Rectangle b) => SetCenter(ref a, b.Center);
		/// <summary>
		/// Sets rectangle a such that a and b share the same center
		/// </summary>
		/// <param name="a">The rectangle to shift</param>
		/// <param name="center">The Point to be taken reference of</param>
		public static Point SetCenter(ref Rectangle a, Point center)
		{
			a.X = center.X - (a.Width / 2);
			a.Y = center.Y - (a.Height / 2);
			return a.Location;
		}
		/// <summary>
		/// Creates a rectangle of a given size placed such that its center lies at the given point
		/// </summary>
		/// <param name="center">Where the center of the rectangle should lie</param>
		/// <param name="size">The dimensions of the rectangle</param>
		public static Rectangle SetCenter(Point center, Point size)
		{
			Rectangle x = new(Point.Zero, size);
			SetCenter(ref x, center);
			return x;
		}
		/// <summary>
		/// Sets rectangle a such that a is fitted inside b and both same the same center
		/// </summary>
		/// <param name="a">Rectangle to scale and shift</param>
		/// <param name="b">Rectangle taken as reference</param>
		/// <returns>The final location of the rectangle a</returns>
		public static Point SetCenterScaled(ref Rectangle a, Rectangle b)
		{
			float xScale = b.Width / (float)a.Width;
			float yScale = b.Height / (float)a.Height;
			xScale = xScale < yScale ? xScale : yScale;
			a.Width = (int)(xScale * a.Width);
			a.Height = (int)(xScale * a.Height);
			SetCenter(ref a, b);
			return a.Location;
		}
		/// <summary>Initalizes the Vector2 and Float value of poition and 
		/// scale to fit the text in the rectangle</summary>
		/// <param name="dest">The rectangle to fit the string</param>
		/// <param name="font">The Spritefont object</param>
		/// <param name="message">The content of the string</param>
		/// <param name="scale">The scale to fit</param>
		/// <param name="position">The position of fitting</param>
		public static void FitText(this Rectangle dest, SpriteFont font, string message,
			out float scale, out Vector2 position)
		{
			Vector2 size = font.MeasureString(message);
			float xScale = dest.Width / size.X;
			float yScale = dest.Height / size.Y;
			// Taking the smaller scaling value will result in the text always fitting in the boundaires.
			scale = Math.Min(xScale, yScale);
			// Figure out the location to absolutely-center it in the boundaries rectangle.
			position = dest.Location.ToVector2();
		}
		/// <summary>
		/// Draws the StringObject Instance with the default settings
		/// </summary>
		/// <param name="batch">Spritebatch object</param>
		/// <param name="stringobj">The instance of StringObject</param>
		public static void Draw(this SpriteBatch batch, TextBox stringobj) => stringobj.Draw(batch);
		/// <summary>
		/// Draws the StringObject Instance
		/// </summary>
		/// <param name="batch">SpriteBatch object</param>
		/// <param name="stringObject">The instance of StringObject</param>
		/// <param name="rotation">The angle of rotation</param>
		/// <param name="origin">The origin about which rotation takes place</param>
		public static void Draw(this SpriteBatch batch, TextBox stringObject, float rotation, Vector2 origin)
			=> stringObject.Draw(batch, rotation, origin);
		/// <summary>
		/// 
		/// </summary>
		/// <param name="batch"></param>
		/// <param name="stringObject"></param>
		/// <param name="rotation"></param>
		/// <param name="origin"></param>
		/// <param name="effects"></param>
		public static void Draw(this SpriteBatch batch, TextBox stringObject, float rotation, Vector2 origin,
			SpriteEffects effects) => stringObject.Draw(batch, rotation, origin, effects);

		/// <summary>
		/// Updates an object for the given acceleration and friction
		/// </summary>
		/// <param name="current">The (Vector2,Vector2) Tuple object with elements Velocity and Displacement respectivly</param>
		/// <param name="acc">The acceleration acting on it</param>
		/// <param name="friction">The friction on the body</param>
		public static (Vector2 Velocity, Vector2 Position) Update(
				this (Vector2 Velocity, Vector2 Position) current, Vector2 acc, float friction = 0)
		{
			current.Velocity += acc - (current.Velocity * friction);
			current.Position += current.Velocity;
			return current;
		}
		/// <summary>
		/// Updates an object for the given acceleration and friction
		/// </summary>
		/// <param name="obj2D">The IPhyObj2D object</param>
		/// <param name="acc">The acceleration acting on it</param>
		/// <param name="friction">The friction on the body</param>
		public static void Update(this IPhyObj2D obj2D, Vector2 acc, float friction = 0)
			=> obj2D.Current = Update(obj2D.Current, acc, friction);
		/// <summary>
		/// Updates an object when no acceleration is acting upon it
		/// </summary>
		/// <param name="obj2D">The IPhyObj2D object</param>
		/// <param name="friction">The friction acting upon it</param>
		public static void Update(this IPhyObj2D obj2D, float friction = 0)
			=> Update(obj2D, Vector2.Zero, friction);
		/// <summary>
		/// Updates an object when no acceleration is acting upon it
		/// </summary>
		/// <param name="current">The (Vector2,Vector2) Tuple object with 
		/// elements Velocity and Displacement respectivly</param>
		/// <param name="friction">The friction acting upon it</param>
		public static (Vector2 Velocity, Vector2 Position) Update(
				this (Vector2 Velocity, Vector2 Position) current, float friction = 0)
					=> Update(current, Vector2.Zero, friction);
		/// <summary>
		/// Updates an object when acted upon with the given acceleration and friction
		/// </summary>
		/// <param name="obj3D">The IPhyObj3D</param>
		/// <param name="acc">The acceleration acting upon it</param>
		/// <param name="friction">The friction on the object</param>
		public static void Update(this IPhyObj3D obj3D, Vector3 acc, float friction = 0)
			=> obj3D.Current = Update(obj3D.Current, acc, friction);
		/// <summary>
		/// Updates an object when acted upon with the given acceleration and friction
		/// </summary>
		/// <param name="current">The (Vector3,Vector3) Tuple object with elements 
		/// Velocity and Displacement respectivly</param>
		/// <param name="acc">The acceleration acting upon it</param>
		/// <param name="friction">The friction on the object</param>
		public static (Vector3 Velocity, Vector3 Position) Update(
				this (Vector3 Velocity, Vector3 Position) current, Vector3 acc, float friction = 0)
		{
			current.Velocity += acc - (current.Velocity * friction);
			current.Position += current.Velocity;
			return current;
		}
		/// <summary>
		/// Updates an object when no acceleration acts on it
		/// </summary>
		/// <param name="obj3D">The IPhyObj3D object</param>
		/// <param name="friction">The friction on the body</param>
		public static void Update(this IPhyObj3D obj3D, float friction = 0) => Update(obj3D, Vector3.Zero, friction);
		/// <summary>
		/// Updates an object when no acceleration acts on it
		/// </summary>
		/// <param name="current">The (Vector3,Vector3) Tuple object with elements 
		/// Velocity and Displacement respectivly </param>
		/// <param name="friction">The friction on the body</param>
		public static (Vector3 Velocity, Vector3 Position) Update(
				this (Vector3 Velocity, Vector3 Position) current, float friction = 0)
					=> Update(current, Vector3.Zero, friction);
		/// <summary>
		/// Bounces an object with given position and velocity against a boundary
		/// </summary>
		/// <param name="boundary">The boundary on which the object is supposed to strike</param>
		/// <param name="velocity">The velocity vector object that struck the boundary</param>
		/// <param name="bounds">The rectangle boundary of the object that struck the boundary</param>
		/// <param name="inside">if true, the object is supposed to stay inside the boundary, else outside</param>
		/// <param name="bounceForce">If the object is to be bounced with an additional force. 
		/// If the object is supposed to stop, keep this as 0. If no additional force is </param>
		/// <returns>Returns the side of the <c>bounds</c> rectangle which has bounced</returns>
		public static RectangleSide Bounce(this Rectangle boundary,
			ref Vector2 velocity, ref Rectangle bounds,
				bool inside, float bounceForce = 1)
		{
			RectangleSide side = RectangleSide.None;
			if (inside)
			{
				if (boundary.X > bounds.X)
				{
					side |= RectangleSide.Left;
					bounds.X = boundary.X + 1;
				}
				else if (bounds.X + bounds.Width > boundary.X + boundary.Width)
				{
					side |= RectangleSide.Right;
					bounds.X = boundary.X + boundary.Width - bounds.Width - 1;
				}
				if (boundary.Y > bounds.Y)
				{
					side |= RectangleSide.Top;
					bounds.Y = boundary.Y + 1;
				}
				else if (bounds.Y + bounds.Height > boundary.Y + boundary.Height)
				{
					side |= RectangleSide.Bottom;
					bounds.Y = boundary.Y + boundary.Height - bounds.Height - 1;
				}
			}
			else
			{
				bool li = bounds.X < boundary.X + boundary.Width,
					ri = boundary.X < bounds.X + bounds.Width,
					ti = bounds.Y < boundary.Y + boundary.Height,
					bi = boundary.Y < bounds.Y + bounds.Height;
				if (!li || !ri || !ti || !bi)
					return RectangleSide.None;
				if (boundary.X + boundary.Width < bounds.X + bounds.Width)
				{
					side |= RectangleSide.Left;
					bounds.X = boundary.X + boundary.Width + 1;
				}
				else if (bounds.X < boundary.X)
				{
					side |= RectangleSide.Right;
					bounds.X = boundary.X - bounds.Width - 1;
				}
				else if (boundary.Y + boundary.Height < bounds.Y + bounds.Height)
				{
					side |= RectangleSide.Top;
					bounds.Y = boundary.Y + boundary.Height + 1;
				}
				else if (bounds.Y < boundary.Y)
				{
					side |= RectangleSide.Bottom;
					bounds.Y = boundary.Y - bounds.Height - 1;
				}
			}
			if ((side & (RectangleSide.Left | RectangleSide.Right)) != 0)
				velocity.X = (-velocity.X) * bounceForce;
			if ((side & (RectangleSide.Top | RectangleSide.Bottom)) != 0)
				velocity.Y = (-velocity.Y) * bounceForce;
			return side;
		}
		/// <summary>
		/// Bounces an object with given position and velocity against a boundary
		/// </summary>
		/// <param name="boundary">The boundary on which the object is supposed to strike</param>
		/// <param name="velocity">The velocity vector object that struck the boundary</param>
		/// <param name="position">The position vector object that struck the boundary</param>
		/// <param name="bounds">The rectangle boundary of the object that struck the boundary</param>
		/// <param name="inside">if true, the object is supposed to stay inside the boundary, else outside</param>
		/// <param name="bounceForce">If the object is to be bounced with an additional force. 
		/// If the object is supposed to stop, keep this as 0. If no additional force is </param>
		/// <returns>Returns the side of the <c>bounds</c> rectangle which has bounced</returns>
		public static RectangleSide Bounce(this Rectangle boundary,
			ref Vector2 velocity, ref Vector2 position, ref Rectangle bounds,
				bool inside, float bounceForce = 1)
		{
			bounds.X = (int)position.X;
			bounds.Y = (int)position.Y;
			var side = Bounce(boundary, ref velocity, ref bounds, inside, bounceForce);
			position.X = bounds.X;
			position.Y = bounds.Y;
			return side;
		}
		/// <summary>
		/// Generates a Texture Image from the grid of Colors
		/// </summary>
		/// <param name="grid">The grid of colors to generate image from</param>
		/// <param name="game">The game object whose GraphicsDevice must be used</param>
		/// <returns>The converted texture image</returns>
		public static Texture2D FromColorGrid(this Game game, Color[,] grid)
		{
			int r = grid.GetLength(0), c = grid.GetLength(1);
			Texture2D tex = new(game.GraphicsDevice, r, c);
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
		/// <param name="x0">x coordinate of the first point</param>
		/// <param name="y0">y coordinate of the first point</param>
		/// <param name="x1">x coordinate of the second point</param>
		/// <param name="y1">y coordinate of the second point</param>
		/// <returns>Enumeration of points lying in the line between the points</returns>
		public static IEnumerable<Point> GetPointsOnLine(int x0, int y0, int x1, int y1)
		{
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
		/// <summary>Iterates over all points lying in the lines between the 
		/// two points in argument, using Bresenham line drawing algorithm</summary>
		/// <param name="p0">The first input point</param>
		/// <param name="p1">The second input point</param>
		/// <returns>Enumeration of points lying in the line between the points</returns>
		public static IEnumerable<Point> GetPointsOnLine(Point p0, Point p1) => GetPointsOnLine(p0.X, p0.Y, p1.X, p1.Y);
		/// <summary>Iterates over all points of a circle at a given centre and radius
		/// using the Midpoint Circle drawing algorithm</summary>
		/// <param name="x0">x coordinate of the centre</param>
		/// <param name="y0">y coordinate of the centre</param>
		/// <param name="radius">radius of the circle</param>
		/// <returns>Enumeration of all points on the circle</returns>
		public static IEnumerable<Point> GetPointsOnCircle(int x0, int y0, int radius)
		{
			if (radius <= 1)
				throw new ArgumentException("Radius should be greater than 1", nameof(radius));
			int x = 0, y = radius, d = 3 - (2 * radius), i;
			Point[] ps = new Point[8];
			SetPoints();
			for (i = 0; i < 8; i++)
				yield return ps[i];
			while (y >= x)
			{
				x++;
				if (d > 0)
				{
					y--;
					d = d + 4 * (x - y) + 10;
				}
				else
					d = d + (4 * x) + 6;
				SetPoints();
				for (i = 0; i < 8; i++)
					yield return ps[i];
			}
			void SetPoints()
			{
				ps[0] = new(x0 + x, y0 + y);
				ps[1] = new(x0 + x, y0 - y);
				ps[2] = new(x0 - x, y0 + y);
				ps[3] = new(x0 - x, y0 - y);
				ps[4] = new(x0 + y, y0 + x);
				ps[5] = new(x0 + y, y0 - x);
				ps[6] = new(x0 - y, y0 + x);
				ps[7] = new(x0 - y, y0 - x);
			}
		}
		/// <summary>Iterates over all points of a circle at a given centre and radius</summary>
		/// <param name="p">Centre of the circle</param>
		/// <param name="radius">radius of the circle</param>
		/// <returns>Enumeration of all points on the circle</returns>
		public static IEnumerable<Point> GetPointsOnCircle(Point p, int radius) => GetPointsOnCircle(p.X, p.Y, radius);
	}
}