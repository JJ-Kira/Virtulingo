/*
 * Copyright (c) Meta Platforms, Inc. and affiliates.
 * All rights reserved.
 *
 * Licensed under the Oculus SDK License Agreement (the "License");
 * you may not use the Oculus SDK except in compliance with the License,
 * which is provided at the time of installation or download, or which
 * otherwise accompanies this software in either electronic or hard copy form.
 *
 * You may obtain a copy of the License at
 *
 * https://developer.oculus.com/licenses/oculussdk/
 *
 * Unless required by applicable law or agreed to in writing, the Oculus SDK
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Meta.XR.MRUtilityKit
{

    /// <summary>
    /// This contains helpers to serialize/deserialze Scene data to/from JSON
    /// </summary>
    public static class SerializationHelpers
    {
        [JsonConverter(typeof(StringEnumConverter))]
        public enum CoordinateSystem
        {
            Unity,
            Unreal,
        }

        private struct TransformData
        {
            public Vector3 Translation;
            public Vector3 Rotation;
            public Vector3 Scale;
        }

        private struct PlaneBoundsData
        {
            public Vector2 Min;
            public Vector2 Max;
        }

        private struct VolumeBoundsData
        {
            public Vector3 Min;
            public Vector3 Max;
        }

        private struct AnchorData
        {
            public string UUID;
            public List<string> SemanticClassifications;
            public TransformData Transform;
            public PlaneBoundsData? PlaneBounds;
            public VolumeBoundsData? VolumeBounds;
            public List<Vector2> PlaneBoundary2D;
        }

        private struct RoomLayoutData
        {
            public string FloorUuid;
            public string CeilingUuid;
            public List<string> WallsUUid;
        }

        private struct RoomData
        {
            public string UUID;
            public RoomLayoutData RoomLayout;
            public List<AnchorData> Anchors;
        }

        private struct SceneData
        {
            public CoordinateSystem CoordinateSystem;
            public List<RoomData> Rooms;
        }

        private class Vector2Converter : JsonConverter<Vector2>
        {
            public override void WriteJson(JsonWriter writer, Vector2 value, JsonSerializer serializer)
            {
                writer.WriteStartArray();
                // Disable indentation to make it more compact
                var prevFormatting = writer.Formatting;
                writer.Formatting = Formatting.None;
                writer.WriteValue(value.x);
                writer.WriteValue(value.y);
                writer.WriteEndArray();
                writer.Formatting = prevFormatting;
            }

            public override Vector2 ReadJson(JsonReader reader, Type objectType, Vector2 existingValue, bool hasExistingValue, JsonSerializer serializer)
            {
                Vector2 result = new();
                result.x = (float)reader.ReadAsDouble();
                result.y = (float)reader.ReadAsDouble();
                reader.Read();
                if (reader.TokenType != JsonToken.EndArray)
                {
                    throw new Exception("Expected end of array");
                }
                return result;
            }
        }

        private class Vector3Converter : JsonConverter<Vector3>
        {
            public override void WriteJson(JsonWriter writer, Vector3 value, JsonSerializer serializer)
            {
                writer.WriteStartArray();
                // Disable indentation to make it more compact
                var prevFormatting = writer.Formatting;
                writer.Formatting = Formatting.None;
                writer.WriteValue(value.x);
                writer.WriteValue(value.y);
                writer.WriteValue(value.z);
                writer.WriteEndArray();
                writer.Formatting = prevFormatting;
            }

            public override Vector3 ReadJson(JsonReader reader, Type objectType, Vector3 existingValue, bool hasExistingValue, JsonSerializer serializer)
            {
                Vector3 result = new();
                result.x = (float)reader.ReadAsDouble();
                result.y = (float)reader.ReadAsDouble();
                result.z = (float)reader.ReadAsDouble();
                reader.Read();
                if (reader.TokenType != JsonToken.EndArray)
                {
                    throw new Exception("Expected end of array");
                }
                return result;
            }
        }

        const float UnrealWorldToMeters = 100f;

        public static string Serialize(CoordinateSystem coordinateSystem)
        {
            SceneData sceneData = new();
            sceneData.CoordinateSystem = coordinateSystem;
            sceneData.Rooms = new();

            foreach (var room in MRUK.Instance.GetRooms())
            {
                RoomData roomData = new();
                if (room.Anchor != OVRAnchor.Null)
                {
                    roomData.UUID = room.Anchor.Uuid.ToString("N").ToUpper();
                }
                else
                {
                    roomData.UUID = Guid.NewGuid().ToString("N").ToUpper();
                }
                roomData.RoomLayout = new();
                roomData.RoomLayout.WallsUUid = new();
                roomData.Anchors = new();
                foreach (var anchor in room.GetRoomAnchors())
                {
                    AnchorData anchorData = new();
                    if (anchor.Anchor != OVRAnchor.Null)
                    {
                        anchorData.UUID = anchor.Anchor.Uuid.ToString("N").ToUpper();
                    }
                    else
                    {
                        anchorData.UUID = Guid.NewGuid().ToString("N").ToUpper();
                    }
                    if (anchor == room.GetCeilingAnchor())
                    {
                        roomData.RoomLayout.CeilingUuid = anchorData.UUID;
                    }
                    if (anchor == room.GetFloorAnchor())
                    {
                        roomData.RoomLayout.FloorUuid = anchorData.UUID;
                    }
                    if (room.GetWallAnchors().Contains(anchor))
                    {
                        roomData.RoomLayout.WallsUUid.Add(anchorData.UUID);
                    }
                    anchorData.SemanticClassifications = anchor.AnchorLabels;
                    anchorData.Transform = new();
                    var localPosition = anchor.transform.localPosition;
                    var localRotation = anchor.transform.localEulerAngles;
                    if (coordinateSystem == CoordinateSystem.Unreal)
                    {
                        localPosition = new Vector3(localPosition.z * UnrealWorldToMeters, localPosition.x * UnrealWorldToMeters, localPosition.y * UnrealWorldToMeters);
                        localRotation = new Vector3(localRotation.x, 180f + localRotation.y, localRotation.z);
                    }
                    anchorData.Transform.Translation = localPosition;
                    anchorData.Transform.Rotation = localRotation;
                    anchorData.Transform.Scale = anchor.transform.localScale;
                    if (anchor.HasPlane)
                    {
                        var min = anchor.PlaneRect.Value.min;
                        var max = anchor.PlaneRect.Value.max;
                        if (coordinateSystem == CoordinateSystem.Unreal)
                        {
                            anchorData.PlaneBounds = new PlaneBoundsData()
                            {
                                Min = new Vector2(-max.x * UnrealWorldToMeters, min.y * UnrealWorldToMeters),
                                Max = new Vector2(-min.x * UnrealWorldToMeters, max.y * UnrealWorldToMeters),
                            };
                        }
                        else
                        {
                            anchorData.PlaneBounds = new PlaneBoundsData()
                            {
                                Min = min,
                                Max = max,
                            };
                        }
                    }
                    if (anchor.PlaneBoundary2D != null)
                    {
                        anchorData.PlaneBoundary2D = new();
                        anchorData.PlaneBoundary2D.Capacity = anchor.PlaneBoundary2D.Count;
                        if (coordinateSystem == CoordinateSystem.Unreal)
                        {
                            foreach (var p in anchor.PlaneBoundary2D)
                            {
                                anchorData.PlaneBoundary2D.Add(new Vector2(-p.x * UnrealWorldToMeters, p.y * UnrealWorldToMeters));
                            }
                            anchorData.PlaneBoundary2D.Reverse();
                        }
                        else
                        {
                            anchorData.PlaneBoundary2D = anchor.PlaneBoundary2D;
                        }
                    }
                    if (anchor.HasVolume)
                    {
                        var min = anchor.VolumeBounds.Value.min;
                        var max = anchor.VolumeBounds.Value.max;
                        if (coordinateSystem == CoordinateSystem.Unreal)
                        {
                            anchorData.VolumeBounds = new VolumeBoundsData()
                            {
                                Min = new Vector3(-max.z * UnrealWorldToMeters, min.x * UnrealWorldToMeters, min.y * UnrealWorldToMeters),
                                Max = new Vector3(-min.z * UnrealWorldToMeters, max.x * UnrealWorldToMeters, max.y * UnrealWorldToMeters),
                            };
                        }
                        else
                        {
                            anchorData.VolumeBounds = new VolumeBoundsData()
                            {
                                Min = min,
                                Max = max,
                            };
                        }
                    }
                    roomData.Anchors.Add(anchorData);
                }
                sceneData.Rooms.Add(roomData);
            }

            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.NullValueHandling = NullValueHandling.Ignore;
            settings.Formatting = Formatting.Indented;
            settings.Converters = new List<JsonConverter>
            {
                new Vector2Converter(),
                new Vector3Converter(),
            };
            string json = JsonConvert.SerializeObject(sceneData, settings);

            return json;
        }

        // var foo = JsonConvert.DeserializeObject<SceneData>(json, settings);

    }
}
