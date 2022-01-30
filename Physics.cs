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
	/// <summary>
	/// Helper object defining a 2D physics object. Requires to input acceleration for every Update() called
	/// </summary>
	public abstract class PhyObj2D : IPhyObj2D
	{
		/// <summary>These vectors are linked with the 2D motion</summary>
		public Vector2 Position, Velocity, Acceleration;
		/// <summary>The friction value to consider for the acceleration. Must be a non-negative number</summary>
		public float Friction;
		/// <summary>Creates an instance of PhyObj2D with provided friction value</summary>
		/// <param name="fricval"></param>
		public PhyObj2D(float fricval = 0.5f)
		{
			Position = Velocity = Acceleration = Vector2.Zero;
			Friction = fricval;
		}
		/// <summary>
		/// Represents the current Touple of Velocity and Displacement
		/// </summary>
		/// <value></value>
		public (Vector2 V, Vector2 X) Current
		{
			get => (Velocity, Position);
			set
			{
				Velocity = value.V;
				Position = value.X;
			}
		}
		/// <summary>
		/// Draw the gameobject at bounds as destination
		/// </summary>
		/// <param name="gt"></param>
		public abstract void Draw(GameTime gt);
		/// <summary>
		/// Goes in the direction of Velocity (Vector2 v) which is added by Acceleration and subtracted with friction
		/// </summary>
		/// <param name="gt">GameTime argument</param>
		public virtual void Update(GameTime gt)
		{
			Acceleration = Vector2.Subtract(Acceleration, Velocity * Friction);
			Vector2.Add(ref Acceleration, ref Velocity, out Velocity);
			Vector2.Add(ref Velocity, ref Position, out Position);
		}
	}
	/// <summary>Physics Object implementation in 3-Dimensions</summary>
	public abstract class PhyObj3D : IPhyObj3D
	{
		/// <summary>
		/// The vectors x,v and a denote instantaneous values of position, velocity and acceleration respectivly
		/// </summary>
		public Vector3 Position, Velocity, Acceleration;
		/// <summary>The friction value to consider for the acceleration. Must be a non-negative number</summary>
		public float Friction;
		/// <summary>
		/// Creates a new PhyObj3D instance with given Friction value
		/// </summary>
		/// <param name="fricval"></param>
		public PhyObj3D(float fricval = 0.05f)
		{
			Position = Velocity = Acceleration = Vector3.Zero;
			Friction = fricval;
		}
		/// <summary>
		/// Represents the current Touple of Velocity and Displacement
		/// </summary>
		/// <value></value>
		public (Vector3 V, Vector3 X) Current
		{
			get => (Velocity, Position);
			set
			{
				Velocity = value.V;
				Position = value.X;
			}
		}
		/// <summary>
		/// Draw the gameobject at bounds as destination
		/// </summary>
		/// <param name="gt"></param>
		public abstract void Draw(GameTime gt);
		/// <summary>
		/// Goes in the direction of Velocity (Vector2 v) which is added by Acceleration and subtracted with friction
		/// </summary>
		/// <param name="gt">GameTime argument</param>
		public virtual void Update(GameTime gt)
		{
			Acceleration = Vector3.Subtract(Acceleration, Velocity * Friction);
			Vector3.Add(ref Acceleration, ref Velocity, out Velocity);
			Vector3.Add(ref Velocity, ref Position, out Position);
		}
	}
}