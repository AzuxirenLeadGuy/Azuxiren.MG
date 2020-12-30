using Microsoft.Xna.Framework;
namespace Azuxiren.MG
{
	/// <summary>Interface for 2D Newtonian physics-based object</summary>
	public interface IPhyObj2D
	{
		/// <summary>Velocity vector,Positional vector (in that order) of the current object</summary>
		(Vector2 V,Vector2 X) Current{get;set;}
	}
	/// <summary>Interface for 3D Newtonian physics-based object</summary>
	public interface IPhyObj3D
	{
		/// <summary>Velocity vector,Positional vector (in that order) of the current object</summary>
		(Vector3 V,Vector3 X) Current{get;set;}
	}
	/// <summary>
	/// Helper object defining a 2D physics object. Requires to input acceleration for every Update() called
	/// </summary>
	public abstract class PhyObj2D:IPhyObj2D
	{
		/// <summary>These vectors are linked with the 2D motion</summary>
		public Vector2 x,v,a;
		/// <summary>The friction value to consider for the acceleration. DO NOT USE FRICTION = 0</summary>
		public float friction;
		/// <summary>Creates an instance of PhyObj2D with provided friction value</summary>
		/// <param name="fricval"></param>
		public PhyObj2D(float fricval=0.5f)
		{
			x=v=a=Vector2.Zero;
			friction=fricval;
		}
		/// <summary>
		/// Represents the current Touple of Velocity and Displacement
		/// </summary>
		/// <value></value>
		public (Vector2 V, Vector2 X) Current 
		{ 
			get=>(v,x);
			set
			{
				v=value.V;
				x=value.X;
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
			a=Vector2.Subtract(a,v*friction);
			Vector2.Add(ref a,ref v,out v);
			Vector2.Add(ref v,ref x,out x);
		}
	}
	/// <summary>Physics Object implementation in 3-Dimensions</summary>
	public abstract class PhyObj3D:IPhyObj3D
	{
		/// <summary>
		/// The vectors x,v and a denote instantaneous values of position, velocity and acceleration respectivly
		/// </summary>
		public Vector3 x,v,a;
		/// <summary>The friction value to consider for the acceleration. DO NOT USE FRICTION = 0</summary>
		public float friction;
		/// <summary>
		/// Creates a new PhyObj3D instance with given Friction value
		/// </summary>
		/// <param name="fricval"></param>
		public PhyObj3D(float fricval=0.05f)
		{
			x=v=a=Vector3.Zero;
			friction=fricval;
		}
		/// <summary>
		/// Represents the current Touple of Velocity and Displacement
		/// </summary>
		/// <value></value>
		public (Vector3 V, Vector3 X) Current
		{ 
			get => (v,x); 
			set
			{
				v=value.V;
				x=value.X;
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
			a=Vector3.Subtract(a,v*friction);
			Vector3.Add(ref a,ref v,out v);
			Vector3.Add(ref v,ref x,out x);
		}
	}
}