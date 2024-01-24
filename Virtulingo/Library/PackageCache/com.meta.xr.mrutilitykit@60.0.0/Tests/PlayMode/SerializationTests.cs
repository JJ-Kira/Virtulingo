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


using System.Collections;
using UnityEngine;
using NUnit.Framework;
using UnityEngine.TestTools;
using UnityEngine.SceneManagement;
using UnityEditor.SceneManagement;
using System.Text.RegularExpressions;

namespace Meta.XR.MRUtilityKit.Tests
{
    public class SerializationTests : MonoBehaviour
    {
        private const int DefaultTimeoutMs = 10000;
        private MRUKRoom _currentRoom;

        [UnitySetUp]
        public IEnumerator SetUp()
        {
            yield return EditorSceneManager.LoadSceneAsyncInPlayMode("Packages\\com.meta.xr.mrutilitykit\\Tests\\RayCastTests.unity",
                new LoadSceneParameters(LoadSceneMode.Additive));
            yield return new WaitUntil(() => MRUK.Instance.IsInitialized);
            _currentRoom = MRUK.Instance.GetCurrentRoom();
        }

        [UnityTearDown]
        public IEnumerator TearDown()
        {
            for (int i = SceneManager.sceneCount - 1; i >= 1; i--)
            {
                var asyncOperation = SceneManager.UnloadSceneAsync(SceneManager.GetSceneAt(i).name); // Clear/reset scene
                yield return new WaitUntil(() => asyncOperation.isDone);
            }
        }

        /// <summary>
        /// Test that serialization to the Unity coordinate system works as expected.
        /// </summary>
        [UnityTest]
        [Timeout(DefaultTimeoutMs)]
        public IEnumerator SerializationToUnity()
        {
            var json = MRUK.Instance.SaveSceneToJsonString(SerializationHelpers.CoordinateSystem.Unity);
            const string expected = @"{
  ""CoordinateSystem"": ""Unity"",
  ""Rooms"": [
    {
      ""UUID"": ""C8666C49A6BB4A57ADDF678FEE7FC962"",
      ""RoomLayout"": {
        ""FloorUuid"": ""5C8DF31A95DB4167A0ED2C0D780E9EC9"",
        ""CeilingUuid"": ""8EE48685B9EE42F0BE9D934CCFE10ACF"",
        ""WallsUUid"": [
          ""440A7B10D95540E0A76CE6AF4E6598F5"",
          ""418094E1B6B24E7C8729B28FDA2A48AA"",
          ""7CEE005A1A454FC8B629A1DFE19C96D6"",
          ""8C0C135EDBAE40FAA2BE6C6389483A24"",
          ""4946FB935FF8456995C4A62845F18AA9"",
          ""C699771048E44377A7A0107B013D3429"",
          ""C8418B0518B84EA4834D05D040C8FDFD"",
          ""6EBED41B7939401F8825F60038770F97""
        ]
      },
      ""Anchors"": [
        {
          ""UUID"": ""440A7B10D95540E0A76CE6AF4E6598F5"",
          ""SemanticClassifications"": [
            ""WALL_FACE""
          ],
          ""Transform"": {
            ""Translation"": [2.01866651,1.5,2.28662467],
            ""Rotation"": [0.0,269.502716,0.0],
            ""Scale"": [1.0,1.0,1.0]
          },
          ""PlaneBounds"": {
            ""Min"": [-1.83889234,-1.5],
            ""Max"": [1.83889234,1.5]
          },
          ""PlaneBoundary2D"": [
            [-1.83950889,-1.5],
            [1.83950889,-1.5],
            [1.83950889,1.5],
            [-1.83950889,1.5]
          ]
        },
        {
          ""UUID"": ""418094E1B6B24E7C8729B28FDA2A48AA"",
          ""SemanticClassifications"": [
            ""WALL_FACE""
          ],
          ""Transform"": {
            ""Translation"": [0.341732264,1.5,4.269563],
            ""Rotation"": [0.0,184.9589,0.0],
            ""Scale"": [1.0,1.0,1.0]
          },
          ""PlaneBounds"": {
            ""Min"": [-1.66721463,-1.5],
            ""Max"": [1.66721463,1.5]
          },
          ""PlaneBoundary2D"": [
            [-1.66751122,-1.5],
            [1.66751122,-1.5],
            [1.66751122,1.5],
            [-1.66751122,1.5]
          ]
        },
        {
          ""UUID"": ""7CEE005A1A454FC8B629A1DFE19C96D6"",
          ""SemanticClassifications"": [
            ""WALL_FACE""
          ],
          ""Transform"": {
            ""Translation"": [-2.69884,1.5,4.469511],
            ""Rotation"": [0.0,182.31749,0.0],
            ""Scale"": [1.0,1.0,1.0]
          },
          ""PlaneBounds"": {
            ""Min"": [-1.38072717,-1.5],
            ""Max"": [1.38072717,1.5]
          },
          ""PlaneBoundary2D"": [
            [-1.38917828,-1.5],
            [1.38917828,-1.5],
            [1.38917828,1.5],
            [-1.38917828,1.5]
          ]
        },
        {
          ""UUID"": ""8C0C135EDBAE40FAA2BE6C6389483A24"",
          ""SemanticClassifications"": [
            ""WALL_FACE""
          ],
          ""Transform"": {
            ""Translation"": [-4.15937138,1.5,2.96420383],
            ""Rotation"": [0.0,92.9677048,0.0],
            ""Scale"": [1.0,1.0,1.0]
          },
          ""PlaneBounds"": {
            ""Min"": [-1.56323624,-1.5],
            ""Max"": [1.56323624,1.5]
          },
          ""PlaneBoundary2D"": [
            [-1.56851244,-1.5],
            [1.56851244,-1.5],
            [1.56851244,1.5],
            [-1.56851244,1.5]
          ]
        },
        {
          ""UUID"": ""4946FB935FF8456995C4A62845F18AA9"",
          ""SemanticClassifications"": [
            ""WALL_FACE""
          ],
          ""Transform"": {
            ""Translation"": [-2.85284042,1.5,1.33746612],
            ""Rotation"": [0.0,2.706871,0.0],
            ""Scale"": [1.0,1.0,1.0]
          },
          ""PlaneBounds"": {
            ""Min"": [-1.38901389,-1.5],
            ""Max"": [1.38901389,1.5]
          },
          ""PlaneBoundary2D"": [
            [-1.4254365,-1.5],
            [1.4254365,-1.5],
            [1.4254365,1.5],
            [-1.4254365,1.5]
          ]
        },
        {
          ""UUID"": ""C699771048E44377A7A0107B013D3429"",
          ""SemanticClassifications"": [
            ""WALL_FACE""
          ],
          ""Transform"": {
            ""Translation"": [-1.66822481,1.5,-0.307902753],
            ""Rotation"": [0.0,97.31695,0.0],
            ""Scale"": [1.0,1.0,1.0]
          },
          ""PlaneBounds"": {
            ""Min"": [-1.592741,-1.5],
            ""Max"": [1.592741,1.5]
          },
          ""PlaneBoundary2D"": [
            [-1.5999999,-1.5],
            [1.5999999,-1.5],
            [1.5999999,1.5],
            [-1.5999999,1.5]
          ]
        },
        {
          ""UUID"": ""C8418B0518B84EA4834D05D040C8FDFD"",
          ""SemanticClassifications"": [
            ""WALL_FACE""
          ],
          ""Transform"": {
            ""Translation"": [0.03306234,1.5,-1.99241328],
            ""Rotation"": [0.0,3.14845753,0.0],
            ""Scale"": [1.0,1.0,1.0]
          },
          ""PlaneBounds"": {
            ""Min"": [-1.90701389,-1.5],
            ""Max"": [1.90701389,1.5]
          },
          ""PlaneBoundary2D"": [
            [-1.9,-1.5],
            [1.9,-1.5],
            [1.9,1.5],
            [-1.9,1.5]
          ]
        },
        {
          ""UUID"": ""6EBED41B7939401F8825F60038770F97"",
          ""SemanticClassifications"": [
            ""WALL_FACE""
          ],
          ""Transform"": {
            ""Translation"": [1.98591208,1.5,-0.82467556],
            ""Rotation"": [0.0,272.192383,0.0],
            ""Scale"": [1.0,1.0,1.0]
          },
          ""PlaneBounds"": {
            ""Min"": [-1.27340925,-1.5],
            ""Max"": [1.27340925,1.5]
          },
          ""PlaneBoundary2D"": [
            [-1.26950109,-1.5],
            [1.26950109,-1.5],
            [1.26950109,1.5],
            [-1.26950109,1.5]
          ]
        },
        {
          ""UUID"": ""5A948ED9EEF843228A2690AD79303904"",
          ""SemanticClassifications"": [
            ""OTHER""
          ],
          ""Transform"": {
            ""Translation"": [1.61697006,1.0874939,2.351218],
            ""Rotation"": [270.0,5.7210083,0.0],
            ""Scale"": [1.0,1.0,1.0]
          },
          ""VolumeBounds"": {
            ""Min"": [-0.398986936,-0.432495117,-1.08197021],
            ""Max"": [0.398986936,0.432495117,0.0]
          }
        },
        {
          ""UUID"": ""8EA64600C09346729270E5E398305BD1"",
          ""SemanticClassifications"": [
            ""OTHER""
          ],
          ""Transform"": {
            ""Translation"": [0.491380483,0.474517822,3.643852],
            ""Rotation"": [270.0,274.152954,0.0],
            ""Scale"": [1.0,1.0,1.0]
          },
          ""VolumeBounds"": {
            ""Min"": [-0.433013916,-0.410492,-0.468994141],
            ""Max"": [0.433013916,0.410492,0.0]
          }
        },
        {
          ""UUID"": ""3451CB27BCA545B99647E4124204D9E7"",
          ""SemanticClassifications"": [
            ""OTHER""
          ],
          ""Transform"": {
            ""Translation"": [-1.333756,1.27151489,-1.750792],
            ""Rotation"": [270.0,96.27296,0.0],
            ""Scale"": [1.0,1.0,1.0]
          },
          ""VolumeBounds"": {
            ""Min"": [-0.199005172,-0.401000977,-1.26599121],
            ""Max"": [0.199005172,0.401000977,0.0]
          }
        },
        {
          ""UUID"": ""ECEE98BEC182428A9EC19D3AF0AA2278"",
          ""SemanticClassifications"": [
            ""OTHER""
          ],
          ""Transform"": {
            ""Translation"": [-2.335,1.0874939,4.381],
            ""Rotation"": [270.0,1.8795377,0.0],
            ""Scale"": [1.0,1.0,1.0]
          },
          ""VolumeBounds"": {
            ""Min"": [-0.633471549,-0.08175456,-1.08197021],
            ""Max"": [0.633471549,0.08175456,0.0]
          }
        },
        {
          ""UUID"": ""8A4E4448827A492E994E2AC5DBF0CEAA"",
          ""SemanticClassifications"": [
            ""TABLE""
          ],
          ""Transform"": {
            ""Translation"": [-3.72,0.6,2.47],
            ""Rotation"": [270.0,4.0065403,0.0],
            ""Scale"": [1.0,1.0,1.0]
          },
          ""PlaneBounds"": {
            ""Min"": [-0.398986936,-1.0687387],
            ""Max"": [0.398986936,1.0687387]
          },
          ""VolumeBounds"": {
            ""Min"": [-0.398986936,-1.0687387,-0.6],
            ""Max"": [0.398986936,1.0687387,0.0]
          },
          ""PlaneBoundary2D"": [
            [-0.398986936,-1.0687387],
            [0.398986936,-1.0687387],
            [0.398986936,1.0687387],
            [-0.398986936,1.0687387]
          ]
        },
        {
          ""UUID"": ""645D7BCC5CA648B79F1680ADF0747A7F"",
          ""SemanticClassifications"": [
            ""COUCH""
          ],
          ""Transform"": {
            ""Translation"": [0.74,0.5,-1.02],
            ""Rotation"": [270.0,1.23901379,0.0],
            ""Scale"": [1.0,1.0,1.0]
          },
          ""PlaneBounds"": {
            ""Min"": [-1.20065725,-0.9039919],
            ""Max"": [1.20065725,0.9039919]
          },
          ""VolumeBounds"": {
            ""Min"": [-1.20065725,-0.9039919,-0.5],
            ""Max"": [1.20065725,0.9039919,0.0]
          },
          ""PlaneBoundary2D"": [
            [-1.20065725,-0.9039919],
            [1.20065725,-0.9039919],
            [1.20065725,0.9039919],
            [-1.20065725,0.9039919]
          ]
        },
        {
          ""UUID"": ""798F5693DE5841CA9797E1851398F72B"",
          ""SemanticClassifications"": [
            ""WINDOW_FRAME""
          ],
          ""Transform"": {
            ""Translation"": [-1.71,1.576,-0.696],
            ""Rotation"": [0.0,97.4,0.0],
            ""Scale"": [1.0,1.0,1.0]
          },
          ""PlaneBounds"": {
            ""Min"": [-0.646113634,-0.456154764],
            ""Max"": [0.646113634,0.456154764]
          },
          ""PlaneBoundary2D"": [
            [-0.646113634,-0.456154764],
            [0.646113634,-0.456154764],
            [0.646113634,0.456154764],
            [-0.646113634,0.456154764]
          ]
        },
        {
          ""UUID"": ""561ED3DCBF044A9BAA4105C6E546FD43"",
          ""SemanticClassifications"": [
            ""DOOR_FRAME""
          ],
          ""Transform"": {
            ""Translation"": [-1.54,1.03,0.61],
            ""Rotation"": [0.0,97.4,0.0],
            ""Scale"": [1.0,1.0,1.0]
          },
          ""PlaneBounds"": {
            ""Min"": [-0.378175974,-1.00632],
            ""Max"": [0.378175974,1.00632]
          },
          ""PlaneBoundary2D"": [
            [-0.378175974,-1.00632],
            [0.378175974,-1.00632],
            [0.378175974,1.00632],
            [-0.378175974,1.00632]
          ]
        },
        {
          ""UUID"": ""5C8DF31A95DB4167A0ED2C0D780E9EC9"",
          ""SemanticClassifications"": [
            ""FLOOR""
          ],
          ""Transform"": {
            ""Translation"": [-1.06952035,0.0,1.2340889],
            ""Rotation"": [270.0,273.148438,0.0],
            ""Scale"": [1.0,1.0,1.0]
          },
          ""PlaneBounds"": {
            ""Min"": [-3.16107464,-3.18514252],
            ""Max"": [3.16107464,3.18514252]
          },
          ""PlaneBoundary2D"": [
            [-0.614610434,3.142647],
            [-3.1610744,3.18514252],
            [-3.16107464,-0.628885269],
            [0.0159805864,-0.3973336],
            [-0.00542993844,-3.17527866],
            [3.121027,-3.18514228],
            [3.16107488,-0.423978269],
            [3.05573153,2.90878677]
          ]
        },
        {
          ""UUID"": ""8EE48685B9EE42F0BE9D934CCFE10ACF"",
          ""SemanticClassifications"": [
            ""CEILING""
          ],
          ""Transform"": {
            ""Translation"": [-1.06952035,3.0,1.2340889],
            ""Rotation"": [90.0,93.14846,0.0],
            ""Scale"": [1.0,1.0,1.0]
          },
          ""PlaneBounds"": {
            ""Min"": [-3.16107464,-3.18514252],
            ""Max"": [3.16107464,3.18514252]
          },
          ""PlaneBoundary2D"": [
            [-3.05573153,2.90878677],
            [-3.16107488,-0.423978329],
            [-3.121027,-3.185142],
            [0.00542993844,-3.17527866],
            [-0.0159805864,-0.397333622],
            [3.16107464,-0.628885269],
            [3.1610744,3.18514252],
            [0.614610434,3.142647]
          ]
        }
      ]
    }
  ]
}";
            var splitJson = json.Split(new char[] { '\r', '\n' }, System.StringSplitOptions.RemoveEmptyEntries);
            var splitExpected = expected.Split(new char[] { '\r', '\n' }, System.StringSplitOptions.RemoveEmptyEntries);
            for (int i=0; i<splitExpected.Length && i<splitJson.Length; i++)
            {
                if (Regex.IsMatch(splitExpected[i], "[A-F0-9]{32}") &&
                    Regex.IsMatch(splitJson[i], "[A-F0-9]{32}"))
                {
                    // Ignore GUIDs because they change every time
                    continue;
                }
                Assert.AreEqual(splitExpected[i], splitJson[i], "Line {0}", i + 1);
            }
            Assert.AreEqual(splitExpected.Length, splitJson.Length, "Number of lines");
            yield return null;
        }

        /// <summary>
        /// Test that serialization to the Unreal coordinate system works as expected.
        /// </summary>
        [UnityTest]
        [Timeout(DefaultTimeoutMs)]
        public IEnumerator SerializationToUnreal()
        {
            var json = MRUK.Instance.SaveSceneToJsonString(SerializationHelpers.CoordinateSystem.Unreal);
            const string expected = @"{
  ""CoordinateSystem"": ""Unreal"",
  ""Rooms"": [
    {
      ""UUID"": ""3E32B883A9A54B4EACEF12D481CF51BC"",
      ""RoomLayout"": {
        ""FloorUuid"": ""FB7C58605E6747C98E738D03256AA731"",
        ""CeilingUuid"": ""82E300A9C9B2402CAECF2D3C662A124A"",
        ""WallsUUid"": [
          ""77BFE1A1B15647249BABD3DD8B06FE29"",
          ""FF6B11746AEC41DF8631CF01C0B7EB82"",
          ""2594D1F1EF8140389847704830D1ACEC"",
          ""8EA538B3F4E4477A9738A4228D338825"",
          ""591343420BD141DFB23FFEADA3E61D01"",
          ""3CC261B918B542E5B3E053AEC2B3582D"",
          ""8FB498786EEF4301B0650E77464E0C00"",
          ""7DB825CAF36C47EF9DF29F186117C2A6""
        ]
      },
      ""Anchors"": [
        {
          ""UUID"": ""77BFE1A1B15647249BABD3DD8B06FE29"",
          ""SemanticClassifications"": [
            ""WALL_FACE""
          ],
          ""Transform"": {
            ""Translation"": [228.66246,201.866653,150.0],
            ""Rotation"": [0.0,449.502716,0.0],
            ""Scale"": [1.0,1.0,1.0]
          },
          ""PlaneBounds"": {
            ""Min"": [-183.889236,-150.0],
            ""Max"": [183.889236,150.0]
          },
          ""PlaneBoundary2D"": [
            [183.950882,150.0],
            [-183.950882,150.0],
            [-183.950882,-150.0],
            [183.950882,-150.0]
          ]
        },
        {
          ""UUID"": ""FF6B11746AEC41DF8631CF01C0B7EB82"",
          ""SemanticClassifications"": [
            ""WALL_FACE""
          ],
          ""Transform"": {
            ""Translation"": [426.956329,34.1732254,150.0],
            ""Rotation"": [0.0,364.9589,0.0],
            ""Scale"": [1.0,1.0,1.0]
          },
          ""PlaneBounds"": {
            ""Min"": [-166.721466,-150.0],
            ""Max"": [166.721466,150.0]
          },
          ""PlaneBoundary2D"": [
            [166.751129,150.0],
            [-166.751129,150.0],
            [-166.751129,-150.0],
            [166.751129,-150.0]
          ]
        },
        {
          ""UUID"": ""2594D1F1EF8140389847704830D1ACEC"",
          ""SemanticClassifications"": [
            ""WALL_FACE""
          ],
          ""Transform"": {
            ""Translation"": [446.9511,-269.884,150.0],
            ""Rotation"": [0.0,362.3175,0.0],
            ""Scale"": [1.0,1.0,1.0]
          },
          ""PlaneBounds"": {
            ""Min"": [-138.072723,-150.0],
            ""Max"": [138.072723,150.0]
          },
          ""PlaneBoundary2D"": [
            [138.917831,150.0],
            [-138.917831,150.0],
            [-138.917831,-150.0],
            [138.917831,-150.0]
          ]
        },
        {
          ""UUID"": ""8EA538B3F4E4477A9738A4228D338825"",
          ""SemanticClassifications"": [
            ""WALL_FACE""
          ],
          ""Transform"": {
            ""Translation"": [296.42038,-415.937134,150.0],
            ""Rotation"": [0.0,272.9677,0.0],
            ""Scale"": [1.0,1.0,1.0]
          },
          ""PlaneBounds"": {
            ""Min"": [-156.323624,-150.0],
            ""Max"": [156.323624,150.0]
          },
          ""PlaneBoundary2D"": [
            [156.851242,150.0],
            [-156.851242,150.0],
            [-156.851242,-150.0],
            [156.851242,-150.0]
          ]
        },
        {
          ""UUID"": ""591343420BD141DFB23FFEADA3E61D01"",
          ""SemanticClassifications"": [
            ""WALL_FACE""
          ],
          ""Transform"": {
            ""Translation"": [133.746613,-285.284058,150.0],
            ""Rotation"": [0.0,182.706879,0.0],
            ""Scale"": [1.0,1.0,1.0]
          },
          ""PlaneBounds"": {
            ""Min"": [-138.901382,-150.0],
            ""Max"": [138.901382,150.0]
          },
          ""PlaneBoundary2D"": [
            [142.543655,150.0],
            [-142.543655,150.0],
            [-142.543655,-150.0],
            [142.543655,-150.0]
          ]
        },
        {
          ""UUID"": ""3CC261B918B542E5B3E053AEC2B3582D"",
          ""SemanticClassifications"": [
            ""WALL_FACE""
          ],
          ""Transform"": {
            ""Translation"": [-30.7902756,-166.822479,150.0],
            ""Rotation"": [0.0,277.316956,0.0],
            ""Scale"": [1.0,1.0,1.0]
          },
          ""PlaneBounds"": {
            ""Min"": [-159.274109,-150.0],
            ""Max"": [159.274109,150.0]
          },
          ""PlaneBoundary2D"": [
            [159.999985,150.0],
            [-159.999985,150.0],
            [-159.999985,-150.0],
            [159.999985,-150.0]
          ]
        },
        {
          ""UUID"": ""8FB498786EEF4301B0650E77464E0C00"",
          ""SemanticClassifications"": [
            ""WALL_FACE""
          ],
          ""Transform"": {
            ""Translation"": [-199.241333,3.306234,150.0],
            ""Rotation"": [0.0,183.148453,0.0],
            ""Scale"": [1.0,1.0,1.0]
          },
          ""PlaneBounds"": {
            ""Min"": [-190.701385,-150.0],
            ""Max"": [190.701385,150.0]
          },
          ""PlaneBoundary2D"": [
            [190.0,150.0],
            [-190.0,150.0],
            [-190.0,-150.0],
            [190.0,-150.0]
          ]
        },
        {
          ""UUID"": ""7DB825CAF36C47EF9DF29F186117C2A6"",
          ""SemanticClassifications"": [
            ""WALL_FACE""
          ],
          ""Transform"": {
            ""Translation"": [-82.46756,198.5912,150.0],
            ""Rotation"": [0.0,452.192383,0.0],
            ""Scale"": [1.0,1.0,1.0]
          },
          ""PlaneBounds"": {
            ""Min"": [-127.340927,-150.0],
            ""Max"": [127.340927,150.0]
          },
          ""PlaneBoundary2D"": [
            [126.950111,150.0],
            [-126.950111,150.0],
            [-126.950111,-150.0],
            [126.950111,-150.0]
          ]
        },
        {
          ""UUID"": ""E5807E45EC2243EC96ADE49A9D2220A7"",
          ""SemanticClassifications"": [
            ""OTHER""
          ],
          ""Transform"": {
            ""Translation"": [235.1218,161.697,108.74939],
            ""Rotation"": [270.0,185.721008,0.0],
            ""Scale"": [1.0,1.0,1.0]
          },
          ""VolumeBounds"": {
            ""Min"": [0.0,-39.8986931,-43.24951],
            ""Max"": [108.197021,39.8986931,43.24951]
          }
        },
        {
          ""UUID"": ""A677E16ADB3844918C4E92366C0CD351"",
          ""SemanticClassifications"": [
            ""OTHER""
          ],
          ""Transform"": {
            ""Translation"": [364.3852,49.13805,47.4517822],
            ""Rotation"": [270.0,454.152954,0.0],
            ""Scale"": [1.0,1.0,1.0]
          },
          ""VolumeBounds"": {
            ""Min"": [0.0,-43.30139,-41.049202],
            ""Max"": [46.8994141,43.30139,41.049202]
          }
        },
        {
          ""UUID"": ""A18D33413CFA44FD9E27045FD29F895A"",
          ""SemanticClassifications"": [
            ""OTHER""
          ],
          ""Transform"": {
            ""Translation"": [-175.079208,-133.3756,127.151489],
            ""Rotation"": [270.0,276.272949,0.0],
            ""Scale"": [1.0,1.0,1.0]
          },
          ""VolumeBounds"": {
            ""Min"": [0.0,-19.9005165,-40.1000977],
            ""Max"": [126.599121,19.9005165,40.1000977]
          }
        },
        {
          ""UUID"": ""A5886216BE284925B5894FCC6FC8EB27"",
          ""SemanticClassifications"": [
            ""OTHER""
          ],
          ""Transform"": {
            ""Translation"": [438.1,-233.5,108.74939],
            ""Rotation"": [270.0,181.879532,0.0],
            ""Scale"": [1.0,1.0,1.0]
          },
          ""VolumeBounds"": {
            ""Min"": [0.0,-63.3471565,-8.175456],
            ""Max"": [108.197021,63.3471565,8.175456]
          }
        },
        {
          ""UUID"": ""D171A322EC454570BD1890EC4EC6889C"",
          ""SemanticClassifications"": [
            ""TABLE""
          ],
          ""Transform"": {
            ""Translation"": [247.0,-372.0,60.0000038],
            ""Rotation"": [270.0,184.006546,0.0],
            ""Scale"": [1.0,1.0,1.0]
          },
          ""PlaneBounds"": {
            ""Min"": [-39.8986931,-106.873871],
            ""Max"": [39.8986931,106.873871]
          },
          ""VolumeBounds"": {
            ""Min"": [0.0,-39.8986931,-106.873871],
            ""Max"": [60.0000038,39.8986931,106.873871]
          },
          ""PlaneBoundary2D"": [
            [39.8986931,106.873871],
            [-39.8986931,106.873871],
            [-39.8986931,-106.873871],
            [39.8986931,-106.873871]
          ]
        },
        {
          ""UUID"": ""3E57722E8D3C47B7977C01D88ECBC562"",
          ""SemanticClassifications"": [
            ""COUCH""
          ],
          ""Transform"": {
            ""Translation"": [-102.0,74.0,50.0],
            ""Rotation"": [270.0,181.239014,0.0],
            ""Scale"": [1.0,1.0,1.0]
          },
          ""PlaneBounds"": {
            ""Min"": [-120.065727,-90.3991852],
            ""Max"": [120.065727,90.3991852]
          },
          ""VolumeBounds"": {
            ""Min"": [0.0,-120.065727,-90.3991852],
            ""Max"": [50.0,120.065727,90.3991852]
          },
          ""PlaneBoundary2D"": [
            [120.065727,90.3991852],
            [-120.065727,90.3991852],
            [-120.065727,-90.3991852],
            [120.065727,-90.3991852]
          ]
        },
        {
          ""UUID"": ""272D6ABB9A4B4169B231B53A5AEA8858"",
          ""SemanticClassifications"": [
            ""WINDOW_FRAME""
          ],
          ""Transform"": {
            ""Translation"": [-69.6,-171.0,157.599991],
            ""Rotation"": [0.0,277.4,0.0],
            ""Scale"": [1.0,1.0,1.0]
          },
          ""PlaneBounds"": {
            ""Min"": [-64.61137,-45.6154747],
            ""Max"": [64.61137,45.6154747]
          },
          ""PlaneBoundary2D"": [
            [64.61137,45.6154747],
            [-64.61137,45.6154747],
            [-64.61137,-45.6154747],
            [64.61137,-45.6154747]
          ]
        },
        {
          ""UUID"": ""E61EB20C30C948EF886D139EE2DFBF75"",
          ""SemanticClassifications"": [
            ""DOOR_FRAME""
          ],
          ""Transform"": {
            ""Translation"": [61.0,-154.0,103.0],
            ""Rotation"": [0.0,277.4,0.0],
            ""Scale"": [1.0,1.0,1.0]
          },
          ""PlaneBounds"": {
            ""Min"": [-37.8175964,-100.632],
            ""Max"": [37.8175964,100.632]
          },
          ""PlaneBoundary2D"": [
            [37.8175964,100.632],
            [-37.8175964,100.632],
            [-37.8175964,-100.632],
            [37.8175964,-100.632]
          ]
        },
        {
          ""UUID"": ""FB7C58605E6747C98E738D03256AA731"",
          ""SemanticClassifications"": [
            ""FLOOR""
          ],
          ""Transform"": {
            ""Translation"": [123.40889,-106.952034,0.0],
            ""Rotation"": [270.0,453.148438,0.0],
            ""Scale"": [1.0,1.0,1.0]
          },
          ""PlaneBounds"": {
            ""Min"": [-316.107452,-318.514252],
            ""Max"": [316.107452,318.514252]
          },
          ""PlaneBoundary2D"": [
            [-305.573151,290.878662],
            [-316.107483,-42.3978271],
            [-312.1027,-318.514221],
            [0.542993844,-317.527863],
            [-1.5980587,-39.73336],
            [316.107452,-62.8885269],
            [316.107452,318.514252],
            [61.4610443,314.2647]
          ]
        },
        {
          ""UUID"": ""82E300A9C9B2402CAECF2D3C662A124A"",
          ""SemanticClassifications"": [
            ""CEILING""
          ],
          ""Transform"": {
            ""Translation"": [123.40889,-106.952034,300.0],
            ""Rotation"": [90.0,273.148468,0.0],
            ""Scale"": [1.0,1.0,1.0]
          },
          ""PlaneBounds"": {
            ""Min"": [-316.107452,-318.514252],
            ""Max"": [316.107452,318.514252]
          },
          ""PlaneBoundary2D"": [
            [-61.4610443,314.2647],
            [-316.107452,318.514252],
            [-316.107452,-62.8885269],
            [1.5980587,-39.73336],
            [-0.542993844,-317.527863],
            [312.1027,-318.5142],
            [316.107483,-42.3978348],
            [305.573151,290.878662]
          ]
        }
      ]
    }
  ]
}
";
            var splitJson = json.Split(new char[] { '\r', '\n' }, System.StringSplitOptions.RemoveEmptyEntries);
            var splitExpected = expected.Split(new char[] { '\r', '\n' }, System.StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < splitExpected.Length && i < splitJson.Length; i++)
            {
                if (Regex.IsMatch(splitExpected[i], "[A-F0-9]{32}") &&
                    Regex.IsMatch(splitJson[i], "[A-F0-9]{32}"))
                {
                    // Ignore GUIDs because they change every time
                    continue;
                }
                Assert.AreEqual(splitExpected[i], splitJson[i], "Line {0}", i + 1);
            }
            Assert.AreEqual(splitExpected.Length, splitJson.Length, "Number of lines");
            yield return null;
        }
    }
}
