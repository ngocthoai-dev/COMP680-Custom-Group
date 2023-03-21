using Core.Gameplay;
using Core.SO;
using Network.UnityGGPO;
using System;
using System.IO;
using Unity.Collections;
using UnityEngine;

namespace Core.GGPO
{
    [Serializable]
    public struct NetworkGame : IGame
    {
        public int Framenumber { get; private set; }
        public static int Timer { get; set; }
        public static bool Start { get; set; }
        public static bool Run { get; set; }

        public int Checksum => GetHashCode();

        public NetworkCharacter[] Characters { get; private set; }
        public MapConfig MapConfig { get; private set; }

        public NetworkGame(MapConfig mapConfig, CharacterConfigSO[] characterConfigSOs)
        {
            Framenumber = 0;
            Timer = 99;
            MapConfig = mapConfig;
            Characters = new NetworkCharacter[characterConfigSOs.Length];
            for (int i = 0; i < Characters.Length; i++)
            {
                Characters[i] = new NetworkCharacter();
            }
        }

        public void Update(long[] inputs, int disconnect_flags)
        {
            if (Time.timeScale > 0)
            {
                if (Framenumber == 0)
                {
                    Start = true;
                }
                Framenumber++;
                if (Framenumber <= 0) return;
                if (Characters[0].CharacterController == null) return;

                for (int idx = 0; idx < Characters.Length; idx++)
                {
                    bool up = false;
                    bool down = false;
                    bool left = false;
                    bool right = false;
                    bool light = false;
                    bool heavy = false;
                    bool skill1 = false;
                    bool skill2 = false;
                    bool dashForward = false;
                    bool dashBackward = false;
                    if ((disconnect_flags & (1 << idx)) != 0)
                    {
                        //Handle AI
                    }
                    else
                    {
                        NetworkInput.ParseInputs(inputs[idx],
                                              out up,
                                              out down,
                                              out left,
                                              out right,
                                              out light,
                                              out heavy,
                                              out skill1,
                                              out skill2,
                                              out dashForward,
                                              out dashBackward);
                    }
                    Characters[idx].CharacterController.Logic(idx, up, down, left, right, light, heavy, skill1, skill2, dashForward, dashBackward);
                }

                if (!Run || Framenumber % 60 != 0) return;
                if (Timer > 0)
                {
                    Timer--;
                    // End battle
                }
            }
        }

        public long ReadInputs(int id)
        {
            return NetworkInput.GetInput(id);
        }

        public void LogInfo(string filename)
        {
            Debug.Log(filename);
            //string fp = "";
            //fp += "GameState object.\n";
            //fp += string.Format("  bounds: {0},{1} x {2},{3}.\n", _bounds.xMin, _bounds.yMin, _bounds.xMax, _bounds.yMax);
            //fp += string.Format("  num_ships: {0}.\n", _ships.Length);
            //for (int i = 0; i < _ships.Length; i++)
            //{
            //    var ship = _ships[i];
            //    fp += string.Format("  ship {0} position:  %.4f, %.4f\n", i, ship.position.x, ship.position.y);
            //    fp += string.Format("  ship {0} velocity:  %.4f, %.4f\n", i, ship.velocity.x, ship.velocity.y);
            //    fp += string.Format("  ship {0} radius:    %d.\n", i, ship.radius);
            //    fp += string.Format("  ship {0} heading:   %d.\n", i, ship.heading);
            //    fp += string.Format("  ship {0} health:    %d.\n", i, ship.health);
            //    fp += string.Format("  ship {0} cooldown:  %d.\n", i, ship.cooldown);
            //    fp += string.Format("  ship {0} score:     {1}.\n", i, ship.score);
            //    for (int j = 0; j < ship.bullets.Length; j++)
            //    {
            //        fp += string.Format("  ship {0} bullet {1}: {2} {3} -> {4} {5}.\n", i, j,
            //                ship.bullets[j].position.x, ship.bullets[j].position.y,
            //                ship.bullets[j].velocity.x, ship.bullets[j].velocity.y);
            //    }
            //}
            //File.WriteAllText(filename, fp);
        }

        public void Serialize(BinaryWriter bw)
        {
            bw.Write(Framenumber);
            bw.Write(Characters.Length);
            for (int i = 0; i < Characters.Length; ++i)
            {
                Characters[i].Serialize(bw);
            }
        }

        public void Deserialize(BinaryReader br)
        {
            Framenumber = br.ReadInt32();
            int length = br.ReadInt32();
            if (length != Characters.Length)
            {
                Characters = new NetworkCharacter[length];
            }
            for (int i = 0; i < Characters.Length; ++i)
            {
                Characters[i].Deserialize(br);
            }
        }

        public NativeArray<byte> ToBytes()
        {
            using (var memoryStream = new MemoryStream())
            {
                using (var writer = new BinaryWriter(memoryStream))
                {
                    Serialize(writer);
                }
                return new NativeArray<byte>(memoryStream.ToArray(), Allocator.Persistent);
            }
        }

        public void FromBytes(NativeArray<byte> bytes)
        {
            using (var memoryStream = new MemoryStream(bytes.ToArray()))
            {
                using (var reader = new BinaryReader(memoryStream))
                {
                    Deserialize(reader);
                }
            }
        }

        public void FreeBytes(NativeArray<byte> data)
        {
            if (data.IsCreated)
            {
                data.Dispose();
            }
        }

        public override int GetHashCode()
        {
            int hashCode = -1214587014;
            hashCode = hashCode * -1521134295 + Framenumber.GetHashCode();
            foreach (var @char in Characters)
            {
                hashCode = hashCode * -1521134295 + @char.GetHashCode();
            }
            return hashCode;
        }
    }
}