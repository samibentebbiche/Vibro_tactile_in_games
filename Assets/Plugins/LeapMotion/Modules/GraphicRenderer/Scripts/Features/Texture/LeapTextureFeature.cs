/******************************************************************************
 * Copyright (C) Ultraleap, Inc. 2011-2020.                                   *
 * Ultraleap proprietary and confidential.                                    *
 *                                                                            *
 * Use subject to the terms of the Leap Motion SDK Agreement available at     *
 * https://developer.leapmotion.com/sdk_agreement, or another agreement       *
 * between Ultraleap and you, your company or other organization.             *
 ******************************************************************************/

using System;
using UnityEngine;
using UnityEngine.Rendering;
#if UNITY_EDITOR
using UnityEditor;
#endif
using Leap.Unity.Attributes;

namespace Leap.Unity.GraphicalRenderer {

  [LeapGraphicTag("Texture", 10)]
  [Serializable]
  public class LeapTextureFeature : LeapGraphicFeature<LeapTextureData> {

    [Delayed]
    [EditTimeOnly]
    public string propertyName = "_MainTex";
    
    [EditTimeOnly]
    public UVChannelFlags channel = UVChannelFlags.UV0;
  }
}
