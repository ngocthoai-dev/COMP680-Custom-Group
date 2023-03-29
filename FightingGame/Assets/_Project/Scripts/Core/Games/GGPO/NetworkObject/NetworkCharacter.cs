using Core.Gameplay;
using Core.SO;
using System;
using System.IO;
using UnityEngine;

namespace Core.GGPO
{
    [Serializable]
    public class NetworkCharacter : INetworkObject
    {
        public CharacterController2D CharacterController;
        public CharacterConfigSO CharacterConfigSO;
        public Vector2 Position;
        public Vector2 Velocity;
        public int HP;
        public int MP;

        public void Serialize(BinaryWriter bw)
        {
            bw.Write(CharacterController.transform.position.x);
            bw.Write(CharacterController.transform.position.y);
        }

        public void Deserialize(BinaryReader br)
        {
            CharacterController.transform.position = new Vector2(br.ReadSingle(), br.ReadSingle());
        }

        public override int GetHashCode()
        {
            int hashCode = 1858597544;
            hashCode = hashCode * -1521134295 + CharacterController.transform.position.GetHashCode();
            return hashCode;
        }
    }
}