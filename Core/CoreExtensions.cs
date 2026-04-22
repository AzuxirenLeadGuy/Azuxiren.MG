using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

using Microsoft.Xna.Framework;

namespace Azuxiren.MG.Core;

/// <summary>Global extensions for Core functionalities</summary>
public static class CoreExtensions
{
	/// <summary>Returns the squared distance from given two points</summary>
	/// <param name="lhs">Argument for the distance function</param>
	/// <param name="rhs">Argument for the distance function</param>
	/// <returns>The squared distance between two points</returns>
	public static ulong DistanceSquared(this Point lhs, Point rhs)
	{
		Point result = lhs - rhs;
		// convert to ulong to prevent multiplication overflow
		ulong dx = (ulong)result.X, dy = (ulong)result.Y;
		return (dx * dx) + (dy * dy);
	}

	/// <summary>Returns the squared distance from given two points</summary>
	/// <param name="lhs">Argument for the distance function</param>
	/// <param name="rhs">Argument for the distance function</param>
	/// <returns>The squared distance between two points</returns>
	public static double DistanceSquared(this Vector2 lhs, Vector2 rhs)
	{
		Vector2 result = lhs - rhs;
		// convert to ulong to prevent multiplication overflow
		double dx = result.X, dy = result.Y;
		return (dx * dx) + (dy * dy);
	}

	/// <summary>
	/// Evalutes the dot product of two vectors, which
	/// is also equivalent to |lhs|.|rhs|.cos (theta)
	/// where theta is the angle between the vectors
	/// </summary>
	/// <param name="lhs">The left operand</param>
	/// <param name="rhs">The right operand</param>
	/// <returns>The value of dot product, also equivalent to |lhs|.|rhs|.cos (theta)</returns>
	public static long DotSafe(this Point lhs, Point rhs) =>
		((long)lhs.X * rhs.X) +
		((long)lhs.Y * rhs.Y);

	/// <summary>
	/// Evalutes the dot product of two vectors, without overflowing
	/// , which is also equivalent to |lhs|.|rhs|.cos (theta)
	/// where theta is the angle between the vectors
	/// </summary>
	/// <param name="lhs">The left operand</param>
	/// <param name="rhs">The right operand</param>
	/// <returns>The value of dot product, also equivalent to |lhs|.|rhs|.cos (theta)</returns>
	public static double DotSafe(this Vector2 lhs, Vector2 rhs) =>
		((double)lhs.X * rhs.X) +
		((double)lhs.Y * rhs.Y);

	/// <summary>
	/// Evalutes the dot product of two vectors, which
	/// is also equivalent to |lhs|.|rhs|.cos (theta)
	/// where theta is the angle between the vectors
	/// </summary>
	/// <param name="lhs">The left operand</param>
	/// <param name="rhs">The right operand</param>
	/// <returns>The value of dot product, also equivalent to |lhs|.|rhs|.cos (theta)</returns>
	public static double DotSafe(this Vector3 lhs, Vector3 rhs) =>
		((double)lhs.X * rhs.X) +
		((double)lhs.Y * rhs.Y) +
		((double)lhs.Z * rhs.Z);

	/// <summary>
	/// Returns the 2d cross product of vectors, 
	/// also equivaluent to |lhs|.|rhs|.sin (theta)
	/// where theta is the angle between the vectors
	/// </summary>
	/// <param name="lhs">The left operand</param>
	/// <param name="rhs">The right operand</param>
	/// <returns>The value of cross product, also equivalent to |lhs|.|rhs|.sin (theta)</returns>
	public static long WedgeProduct2d(this Point lhs, Point rhs) =>
		((long)lhs.X * rhs.Y) -
		((long)lhs.Y * rhs.X);

	/// <summary>
	/// Returns the 2d wedge product of vectors, 
	/// also equivaluent to |lhs|.|rhs|.sin theta
	/// where theta is the angle between the vectors
	/// </summary>
	/// <param name="lhs">The left operand</param>
	/// <param name="rhs">The right operand</param>
	/// <returns>The value of cross product, also equivalent to |lhs|.|rhs|.sin (theta)</returns>
	public static double WedgeProduct2d(this Vector2 lhs, Vector2 rhs) => ((double)lhs.X * rhs.Y) - ((double)lhs.Y * rhs.X);

	/// <summary>Computes the angle between two vectors</summary>
	/// <param name="lhs">The left operand</param>
	/// <param name="rhs">The right operand</param>
	/// <returns>The angle in radians between the two vectors</returns>
	public static double AngleBetween(this Vector2 lhs, Vector2 rhs)
		=> double.Atan2(
			lhs.WedgeProduct2d(rhs),
			Vector2.Dot(lhs, rhs)
		);

	/// <summary>Computes the angle between two vectors</summary>
	/// <param name="lhs">The left operand</param>
	/// <param name="rhs">The right operand</param>
	/// <returns>The angle in radians between the two vectors</returns>
	public static float AngleBetween(this Point lhs, Point rhs)
		=> float.Atan2(
			lhs.WedgeProduct2d(rhs),
			lhs.DotSafe(rhs)
		);


	/// <summary>Sets the angle in the bracket [-pi, pi]</summary>
	/// <param name="angle">The angle to set</param>
	/// <returns>The angle set between [-pi, pi]</returns>
	public static float AngleMod(float angle) => float.Ieee754Remainder(angle, float.Pi * 2);

	/// <summary>The smallest rectangle which bounds/contains this circle</summary>
	public static Rectangle OuterBound(this IntCircle cicle) => new(
		cicle.Center.X - cicle.Radius,
		cicle.Center.Y - cicle.Radius,
		2 * cicle.Radius,
		2 * cicle.Radius
	);

	/// <summary>
	/// The largest rectangle bounded/contained by this circle. 
	/// Note that this method is subject to inaccuracies due
	/// to floating point conversions
	/// </summary>
	public static Rectangle InnerBound(this IntCircle cicle)
	{
		int half_side = (int)MathF.Round(cicle.Radius * 0.707106781f);
		return new
		(
			cicle.Center.X - half_side,
			cicle.Center.Y - half_side,
			2 * half_side,
			2 * half_side
		);
	}

	/// <summary>Checks if two circles intersect.<br/>
	/// This is a commutative operation.
	/// i.e x.Intersect(y) is always equal to y.Intersect(x)
	/// for any two circles x, y <br/>
	/// If one circle is inside the other
	/// </summary>
	/// <param name="circle">The circle with which we wish to know the intersection</param>
	/// <param name="other">The other circle for which we wish to know the intersection</param>
	/// <returns>true if the circles intersect; false otherwise</returns>
	public static bool Intersect(this IntCircle circle, IntCircle other)
	{
		ulong rad_sum = (ulong)(circle.Radius + other.Radius);
		return circle.Center.DistanceSquared(other.Center) <= (rad_sum * rad_sum);
	}

	/// <summary>
	/// Checks if this circle contains the other circle. <br/><br/>
	/// This is not a commutative operation,
	/// i.e x.Contain(y) is not always equal to y.Contain(x) <br/>
	/// for any two circles x, y
	/// </summary>
	/// <param name="circle">The bigger circle</param>
	/// <param name="other">The other circle to check if it is inside</param>
	/// <returns>true if `other` circle is contained within `this` circle; false otherwise</returns>
	public static bool Contain(this IntCircle circle, IntCircle other)
	{
		ulong rad_diff = (ulong)(circle.Radius - other.Radius);
		return circle.Radius >= other.Radius && // Cannot have bigger circle inside smaller circle
			circle.Center.DistanceSquared(other.Center) <= (rad_diff * rad_diff);
	}

	/// <summary>Checks if a given polygon represented as a sequence of points lies inside an IntCircle</summary>
	/// <param name="circle">The circle to query</param>
	/// <param name="polygonEndpoints">The ordered endpoints of the polygon</param>
	/// <returns>true if all points are inside the circle, otherwise false</returns>
	public static bool Contains(this IntCircle circle, IEnumerable<Point> polygonEndpoints)
	{
		ulong max_sq_dist = polygonEndpoints.Max( // Find the polygon endpoint farthest from the center
			point => point.DistanceSquared(circle.Center)
		), rad = (ulong)circle.Radius;
		return max_sq_dist <= (rad * rad);
	}

	/// <summary>
	/// Checks if a given polygon represented as a sequence of 
	/// points overlaps with the interior of an IntCircle. Note:
	/// a polygon completely inside a circle is noted as having
	/// an overlap.
	/// </summary>
	/// <param name="circle">The circle to query</param>
	/// <param name="polygonEndpoints">The ordered sequence of endpoints of the polygon</param>
	/// <returns></returns>
	public static bool Intersects(this IntCircle circle, IEnumerable<Point> polygonEndpoints)
	{
		// Find the distance of polygon endpoint closest from the center
		ulong squared_dist = polygonEndpoints.Min(
			point => point.DistanceSquared(circle.Center)
		), rad = (ulong)circle.Radius;
		return squared_dist <= (rad * rad);
	}

	/// <summary>
	/// Checks if a given polygon represented as a sequence of 
	/// points overlaps with the interior of an IntCircle. Note:
	/// a polygon completely inside a circle is noted as having
	/// an overlap.
	/// </summary>
	/// <param name="circle">The circle to query</param>
	/// <param name="polygonEndpoints">The ordered sequence of endpoints of the polygon</param>
	/// <returns></returns>
	public static bool Intersects(this Circle circle, IEnumerable<Vector2> polygonEndpoints)
	{
		// Find the distance of polygon endpoint closest from the center
		double squared_dist = polygonEndpoints.Min(
			point => point.DistanceSquared(circle.Center)
		), rad = (ulong)circle.Radius;
		return squared_dist <= (rad * rad);
	}

	/// <summary>A circle that passes through all endpoints of the polygon</summary>
	/// <param name="intPolygon">The polygon to query</param>
	/// <returns>Circle instance</returns>
	public static IntCircle Circumcircle(this IntPolygon intPolygon) => new()
	{
		Center = intPolygon.Center,
		Radius = intPolygon.Radius,
	};

	/// <summary>The largest circle that is contained within the polygon</summary>
	/// <param name="intPolygon">The polygon to query</param>
	/// <returns>Circle instance</returns>
	public static IntCircle Incircle(this IntPolygon intPolygon) => new()
	{
		Center = intPolygon.Center,
		Radius = (int)float.Round(
			intPolygon.Radius * float.CosPi(
				1F / intPolygon.SideCount
			)
		),
	};

	/// <summary>Return the integer points of all the endpoints of the vector</summary>
	/// <param name="intPolygon">The polygon to get endpoints of</param>
	/// <returns>Enumeration of all interger-rounded points of the polygon</returns>
	public static IEnumerable<Point> Endpoints(this IntPolygon intPolygon)
	{
		float tAngle = 2 * float.Pi / intPolygon.SideCount;
		float cur_angle = intPolygon.Angle;
		Vector2 startVec = new(0, intPolygon.Radius);
		for (int idx = 0; idx < intPolygon.SideCount; idx++)
		{
			yield return Vector2.Round(
				Vector2.Rotate
				(
					startVec,
					cur_angle
				)
			).ToPoint() + intPolygon.Center;
			cur_angle += tAngle;
		}
	}

	/// <summary>Checks if a given circle is completely contained within the circle or not</summary>
	/// <param name="circle">The circle to check if it is completely contained</param>
	/// <param name="other">The circle to check if it is completely contained</param>
	/// <returns>True if the Circle lies inside the circle; False otherwise</returns>
	public static bool Contain(this Circle circle, Circle other)
	{
		double rad_diff = circle.Radius - other.Radius;
		return rad_diff >= 0 && // Cannot have larger circle inside smaller circle
			circle.Center.DistanceSquared(other.Center) <= (rad_diff * rad_diff);
	}

	/// <summary>
	/// Checks if a given polygon(given as an ordered sequence of points)
	/// is completely contained within the circle.
	/// </summary>
	/// <param name="circle">The cicle to query</param>
	/// <param name="polygonPoints">The ordered sequence of points to query</param>
	/// <returns>Returns true if all points of the polygon are inside the circle</returns>
	public static bool Contains(this Circle circle, IEnumerable<Vector2> polygonPoints)
	{
		// Find the distance of the polygon endpoint farthest from the center
		double max_sq_dist = polygonPoints.Max(
			point => circle.Center.DistanceSquared(point)
		), rad = circle.Radius;
		return max_sq_dist <= (rad * rad);
	}

	/// <summary>
	/// Checks if a given polygon(given as an ordered sequence of points)
	/// has an overlap with the circle
	/// </summary>
	/// <param name="circle">The cicle to query</param>
	/// <param name="polygonPoints">The ordered sequence of points to query</param>
	/// <returns>Returns true if all points of the polygon are inside the circle</returns>
	public static bool Intersect(this Circle circle, IEnumerable<Vector2> polygonPoints)
	{
		// Find the distance of polygon endpoint closest from the center
		double max_sq_dist = polygonPoints.Min(
			point => circle.Center.DistanceSquared(point)
		), rad = circle.Radius;
		return max_sq_dist <= (rad * rad);
	}

	/// <summary>
	/// Checks if two circles intersect.
	/// If one circle is inside the other, Intersection is still true
	/// </summary>
	/// <param name="circle">The queried circle</param>
	/// <param name="other">The queried circle</param>
	/// <returns>true if the circles intersect; false otherwise</returns>
	public static bool Intersect(this Circle circle, Circle other)
	{
		double rad_sum = circle.Radius + other.Radius;
		return circle.Center.DistanceSquared(other.Center) <= (rad_sum * rad_sum);
	}

	/// <summary>The smallest circle passing through all vertices of the polygon</summary>
	public static Circle CircumCircle(this Polygon polygon) => new()
	{
		Center = polygon.Center,
		Radius = polygon.Radius,
	};

	/// <summary>The largest circle tangent to all sides of the polygon</summary>
	public static Circle InCircle(this Polygon polygon) => new()
	{
		Center = polygon.Center,
		Radius = polygon.Radius * float.CosPi(1f / polygon.SideCount),
	};

	/// <summary>Enumeration of endpoints of the polygon</summary>
	public static IEnumerable<Vector2> EndPoints(this Polygon polygon)
	{
		float tAngle = 2 * float.Pi / polygon.SideCount, cur_angle = polygon.Angle;
		Vector2 startVec = new(0, polygon.Radius);
		for (int idx = 0; idx < polygon.SideCount; idx++)
		{
			yield return polygon.Center + Vector2.Rotate
			(
				startVec,
				cur_angle
			);
			cur_angle += tAngle;
		}
	}

	/// <summary>Tests if the given points are collinear</summary>
	/// <param name="pointA">The point to test</param>
	/// <param name="pointB">The point to test</param>
	/// <param name="pointC">The point to test</param>
	/// <returns>true if the points are collinear, otherwise false</returns>
	public static bool Collinear(Vector2 pointA, Vector2 pointB, Vector2 pointC) =>
		(pointB - pointA).WedgeProduct2d(pointC - pointB) == 0;

	/// <summary>Tests if the given points are collinear</summary>
	/// <param name="pointA">The point to test</param>
	/// <param name="pointB">The point to test</param>
	/// <param name="pointC">The point to test</param>
	/// <returns>true if the points are collinear, otherwise false</returns>
	public static bool Collinear(Point pointA, Point pointB, Point pointC) =>
		(pointB - pointA).WedgeProduct2d(pointC - pointB) == 0;

	/// <summary>Return the update milliseconds count</summary>
	/// <param name="time">The current time elasped as a GameTime instance</param>
	/// <param name="curState">The current acumulated time as uint</param>
	/// <returns>The updated time in milliseconds</returns>
	public static uint UpdateMs([NotNull] this GameTime time, uint curState) =>
		curState + (uint)time.ElapsedGameTime.Milliseconds;

	/// <summary>Return the update milliseconds count</summary>
	/// <param name="time">The current time elasped as a GameTime instance</param>
	/// <param name="curState">The current acumulated time as uint</param>
	/// <returns>The updated time in milliseconds</returns>
	public static double UpdateMs([NotNull] this GameTime time, double curState) =>
		curState + time.ElapsedGameTime.TotalMilliseconds;
}