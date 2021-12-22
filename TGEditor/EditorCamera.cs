using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using PloobsEngine.Engine;
using PloobsEngine.Cameras;

namespace TGEditor
{
    public class EditorCamera : ICamera
    {
        float speed = 3;
        float aspectRatio;

        EngineStuff engine;

        //Camera matrices
        public Matrix view;
        public Matrix projection;

        // Camera vectors
        public Vector3 cameraPosition { get; protected set; }
        Vector3 cameraDirection;
        Vector3 cameraUp;
        Vector3 rotationCenter;
        BoundingSphere rotaionCenterSphere;

        // Mouse support
        MouseState prevMouseState;

        BoundingFrustum boundingFrustrum;
        bool hasMoved = true;

        // Used to set the rotation spheres position
        bool hasSetRotationCenter;

        public Vector3 GetCameraDirection
        {
            get { return cameraDirection; }
        }
 
        public override Matrix View 
        {
            get { return view; }
        }
        public override Matrix Projection 
        {
            get { return projection; } 
        }
        public override Matrix ViewProjection
        {
            get { return view * projection; }
        }

        public override bool Hasmoved
        {
            get { throw new NotImplementedException(); }
        }

        public override BoundingFrustum BoundingFrustum
        {
            get
            {
                if (hasMoved)
                {
                    this.boundingFrustrum = new BoundingFrustum(view * projection);
                    return this.boundingFrustrum;
                }
                return this.boundingFrustrum;
            }
        }

        public override void GetObjectData(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
        {
            throw new NotImplementedException();
        }

        public override float AspectRatio
        {
            get
            {
                return aspectRatio;
            }
            set
            {
                aspectRatio = value;
            }
        }

        public EditorCamera(EngineStuff engine,Editor editorForm, Vector3 pos, Vector3 target, Vector3 up)
        {
            this.engine = engine;

            editorForm.MouseWheel += new System.Windows.Forms.MouseEventHandler(WindowsForm_MouseWheel);

            // Build camera view matrix
            cameraPosition = pos;
            cameraDirection = target - pos;
            cameraDirection.Normalize();
            cameraUp = up;
            CreateLookAt();
          
            projection = Matrix.CreatePerspectiveFieldOfView(
                MathHelper.PiOver4,
                (float)editorForm.Width /
                (float)editorForm.Height,
                1f, 3000f);

            rotationCenter = Vector3.Zero;

            rotaionCenterSphere = new BoundingSphere(rotationCenter, 5f);
            //RasterizerState rs = new RasterizerState();
            //rs.CullMode = CullMode.CullClockwiseFace;
            //EngineStart.engine.GraphicsDevice.RasterizerState = rs;
        }

        public void Initialize()
        {
            aspectRatio = (float)engine.Window.ClientBounds.Width / (float)engine.Window.ClientBounds.Height / 2;

            prevMouseState = Mouse.GetState();
        }
        
        protected override void Update(GameTime gt)
        {
 	        KeyboardState keyboard = Keyboard.GetState();

            if (Mouse.GetState().MiddleButton == ButtonState.Pressed)
            {
                Vector3 pos = cameraPosition;
                if (Keyboard.GetState().IsKeyUp(Keys.LeftAlt))
                    pos.Y -= -(Mouse.GetState().Y - prevMouseState.Y) * 2; // Vertical movement
                else if (Keyboard.GetState().IsKeyDown(Keys.LeftAlt))
                    pos.Y -= (Vector3.Cross(cameraDirection, Vector3.Cross(cameraDirection, cameraUp))).Y * (Mouse.GetState().Y - prevMouseState.Y) * 3.5f;
    
                pos -= -(Vector3.Cross(cameraUp, cameraDirection) * (Mouse.GetState().X - prevMouseState.X)) * 3.5f; // Horizontal movement
                cameraPosition = pos;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.LeftAlt) && Mouse.GetState().MiddleButton == ButtonState.Pressed)
            {
                Ray directionRay = DeferredScreen.mouseRay;
                Plane defaultPlane = new Plane(new Vector4(0, 1, 0, 0));
                if (directionRay.Intersects(defaultPlane).HasValue && !hasSetRotationCenter)
                {
                    Vector3 intersectPoint = directionRay.Position + directionRay.Direction * directionRay.Intersects(defaultPlane).Value;
                    rotaionCenterSphere.Center = intersectPoint;
                    hasSetRotationCenter = true;
                }
                cameraDirection = rotaionCenterSphere.Center - cameraPosition;
                cameraDirection.Normalize();
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.LeftAlt) && hasSetRotationCenter == true)
            {
                cameraDirection = rotaionCenterSphere.Center - cameraPosition;
                cameraDirection.Normalize();
            }
            else
                hasSetRotationCenter = false;

            // Move forward/backward
            if (Keyboard.GetState().IsKeyDown(Keys.T))
                cameraPosition += cameraDirection * speed;
            if (Keyboard.GetState().IsKeyDown(Keys.G))
                cameraPosition -= cameraDirection * speed;
            // Move side to side
            if (Keyboard.GetState().IsKeyDown(Keys.F))
                cameraPosition += Vector3.Cross(cameraUp, cameraDirection) * speed;
            if (Keyboard.GetState().IsKeyDown(Keys.H))
                cameraPosition -= Vector3.Cross(cameraUp, cameraDirection) * speed;

            // Yaw rotation
            if (Mouse.GetState().RightButton == ButtonState.Pressed)
            {
                cameraDirection = Vector3.Transform(cameraDirection,
                    Matrix.CreateFromAxisAngle(cameraUp, (-MathHelper.PiOver4 / 150) *
                    (Mouse.GetState().X - prevMouseState.X)));
            }

            // Pitch rotation
            if (Mouse.GetState().RightButton == ButtonState.Pressed)
            {
                cameraDirection = Vector3.Transform(cameraDirection,
                Matrix.CreateFromAxisAngle(Vector3.Cross(cameraUp, cameraDirection),
                (MathHelper.PiOver4 / 100) *
                (Mouse.GetState().Y - prevMouseState.Y)));
            }


                //Reset mouse state
                prevMouseState = Mouse.GetState();

                // Recreate the camera view matrix
                CreateLookAt();

                if (keyboard.IsKeyDown(Keys.Escape))
                {
                    engine.Exit();
                }
                if(Mouse.GetState().MiddleButton == ButtonState.Released && Keyboard.GetState().IsKeyUp(Keys.LeftAlt))
                    rotaionCenterSphere = new BoundingSphere(Vector3.Zero, 5f);
                DebugShapeRenderer.AddBoundingSphere(rotaionCenterSphere, Color.Turquoise);  
        }

        private void CreateLookAt()
        {
            view = Matrix.CreateLookAt(cameraPosition,
                cameraPosition + cameraDirection, cameraUp);
        }

        void WindowsForm_MouseWheel(object sender, System.Windows.Forms.MouseEventArgs e) // Enables zooming with the mousewheel
        {
            float scroll = e.Delta / 120;
            if (scroll != 0)
            {
                cameraPosition += cameraDirection * scroll * 100;
            }
        }  
    }
}
