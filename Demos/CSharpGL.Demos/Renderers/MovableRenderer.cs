﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;


namespace CSharpGL.Demos
{
    /// <summary>
    /// 可设定其在world space的位置。
    /// </summary>
    internal class MovableRenderer : PickableRenderer
    {
        public static MovableRenderer GetRenderer(IBufferable model)
        {
            var shaderCodes = new ShaderCode[2];
            shaderCodes[0] = new ShaderCode(File.ReadAllText(@"shaders\Teapot.vert"), ShaderType.VertexShader);
            shaderCodes[1] = new ShaderCode(File.ReadAllText(@"shaders\Teapot.frag"), ShaderType.FragmentShader);
            var propertyNameMap = new PropertyNameMap();
            propertyNameMap.Add("in_Position", "position");
            propertyNameMap.Add("in_Color", "color");
            var result = new MovableRenderer(model, shaderCodes, propertyNameMap, "position");
            return result;
        }

        private float radian;

        public float Scale { get; set; }

        public vec3 Position { get; set; }

        public mat4 Rotation { get; set; }

        private MovableRenderer(IBufferable bufferable, ShaderCode[] shaderCodes,
            PropertyNameMap propertyNameMap, string positionNameInIBufferable,
            params GLSwitch[] switches)
            : base(bufferable, shaderCodes, propertyNameMap, "position")
        {
            this.Scale = 1.0f;
        }

        protected override void DoInitialize()
        {
            base.DoInitialize();
        }

        protected override void DoRender(RenderEventArg arg)
        {
            mat4 projection = arg.Camera.GetProjectionMat4();
            mat4 view = arg.Camera.GetViewMat4();
            mat4 model = mat4.identity();
            model = glm.translate(model, this.Position);
            model = glm.scale(model, new vec3(this.Scale, this.Scale, this.Scale));
            model = glm.rotate(model, this.radian, new vec3(0, 1, 0));
            this.SetUniform("projectionMatrix", projection);
            this.SetUniform("viewMatrix", view);
            this.SetUniform("modelMatrix", model);
            base.DoRender(arg);
        }

        internal void SetDirection(vec3 direction)
        {
            direction.y = 0;
            direction = direction.normalize();
            float cosRadian = direction.dot(new vec3(1, 0, 0));
            float radian = (float)Math.Acos(cosRadian);
            if (direction.z > 0) { radian = -radian; }
            this.radian = radian;
        }
    }
}

