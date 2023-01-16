using Microsoft.Xna.Framework;
namespace Azuxiren.MG
{
	/// <summary>Interface for 2D Newtonian physics-based object</summary>
	public interface IPhyObj2D
	{
		/// <summary>Velocity vector,Positional vector (in that order) of the current object</summary>
		(Vector2 V, Vector2 X) Current { get; set; }
	}
	/// <summary>Interface for 3D Newtonian physics-based object</summary>
	public interface IPhyObj3D
	{
		/// <summary>Velocity vector,Positional vector (in that order) of the current object</summary>
		(Vector3 V, Vector3 X) Current { get; set; }
	}
	public partial class Global
	{

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
		/// Updates an object when no acceleration is acting upon it
		/// </summary>
		/// <param name="current">The (Vector2,Vector2) Tuple object with 
		/// elements Velocity and Displacement respectivly</param>
		/// <param name="friction">The friction acting upon it</param>
		public static (Vector2 Velocity, Vector2 Position) Update(
				this (Vector2 Velocity, Vector2 Position) current, float friction = 0)
					=> Update(current, Vector2.Zero, friction);
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
		/// <param name="current">The (Vector3,Vector3) Tuple object with elements 
		/// Velocity and Displacement respectivly </param>
		/// <param name="friction">The friction on the body</param>
		public static (Vector3 Velocity, Vector3 Position) Update(
				this (Vector3 Velocity, Vector3 Position) current, float friction = 0)
					=> Update(current, Vector3.Zero, friction);
		/// <summary>
		/// Updates an object when acted upon with the given acceleration and friction
		/// </summary>
		/// <param name="obj3D">The IPhyObj3D</param>
		/// <param name="acc">The acceleration acting upon it</param>
		/// <param name="friction">The friction on the object</param>
		public static void Update(this IPhyObj3D obj3D, Vector3 acc, float friction = 0)
			=> obj3D.Current = Update(obj3D.Current, acc, friction);
		/// <summary>
		/// Updates an object when no acceleration acts on it
		/// </summary>
		/// <param name="obj3D">The IPhyObj3D object</param>
		/// <param name="friction">The friction on the body</param>
		public static void Update(this IPhyObj3D obj3D, float friction = 0) => Update(obj3D, Vector3.Zero, friction);
	}
}