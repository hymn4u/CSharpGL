﻿using CSharpGL;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;


namespace CSharpGL
{
    /// <summary>
    /// Renders a bounding box.
    /// </summary>
    public class BoundingBoxRenderer : Renderer, IBoundingBox
    {
        /// <summary>
        /// get a bounding box renderer.
        /// </summary>
        /// <param name="lengths">bounding box's length at x, y, z direction.</param>
        /// <returns></returns>
        public static BoundingBoxRenderer GetBoundingBoxRenderer(vec3 lengths)
        {
            var bufferable = new BoundingBoxModel(lengths);
            var shaderCodes = new ShaderCode[2];
            shaderCodes[0] = new ShaderCode(ManifestResourceLoader.LoadTextFile(
                @"Resources\BoundingBox.vert"), ShaderType.VertexShader);
            shaderCodes[1] = new ShaderCode(ManifestResourceLoader.LoadTextFile(
                @"Resources\BoundingBox.frag"), ShaderType.FragmentShader);
            var map = new PropertyNameMap();
            map.Add("in_Position", BoundingBoxModel.strPosition);
            var result = new BoundingBoxRenderer(bufferable, shaderCodes, map, new PolygonModeSwitch(PolygonModes.Lines), new PolygonOffsetFillSwitch());
            result.MaxPosition = lengths / 2;
            result.MinPosition = -lengths / 2;

            return result;
        }

        /// <summary>
        /// Rendering something using GLSL shader and VBO(VAO).
        /// </summary>
        /// <param name="bufferable">model data that can be transfermed into OpenGL Buffer's pointer.</param>
        /// <param name="shaderCodes">All shader codes needed for this renderer.</param>
        /// <param name="propertyNameMap">Mapping relations between 'in' variables in vertex shader in <paramref name="shaderCodes"/> and buffers in <paramref name="bufferable"/>.</param>
        ///<param name="switches">OpenGL switches.</param>
        private BoundingBoxRenderer(IBufferable bufferable, ShaderCode[] shaderCodes,
            PropertyNameMap propertyNameMap, params GLSwitch[] switches)
            : base(bufferable, shaderCodes, propertyNameMap, switches)
        {
            this.BoundingBoxColor = new vec3(1, 1, 1);
        }

        private UpdatingRecord boundingBoxColorRecord = new UpdatingRecord();
        private vec3 boundingBoxColor;
        /// <summary>
        /// 
        /// </summary>
        public vec3 BoundingBoxColor
        {
            get { return boundingBoxColor; }
            set
            {
                if (value != boundingBoxColor)
                {
                    boundingBoxColor = value;
                    boundingBoxColorRecord.Mark();
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="arg"></param>
        protected override void DoRender(RenderEventArg arg)
        {
            //mat4 projection = arg.Camera.GetProjectionMat4();
            //mat4 view = arg.Camera.GetViewMat4();
            //this.SetUniform("projectionMatrix", projection);
            //this.SetUniform("viewMatrix", view);
            //mat4 model = glm.translate(mat4.identity(), this.GetCenter());
            //model = glm.scale(model, this.MaxPosition - this.MinPosition);
            //this.SetUniform("modelMatrix", model);
            if (this.boundingBoxColorRecord.IsMarked())
            {
                this.SetUniform("boundingBoxColor", this.BoundingBoxColor);
                this.boundingBoxColorRecord.CancelMark();
            }

            base.DoRender(arg);
        }

        /// <summary>
        /// 
        /// </summary>
        public vec3 MaxPosition { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public vec3 MinPosition { get; set; }

    }
}
