using Core.Utility;
using Network.UnityGGPO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityGGPO;

namespace Core.GGPO
{
    public class NetworkObjectView : MonoBehaviour
    {
        [SerializeField][DebugOnly] protected TextMeshProUGUI _txtStatus;
        [SerializeField][DebugOnly] protected Image _imgProgress;

        protected virtual void Awake()
        {
            _txtStatus = transform.GetComponentInChildren<TextMeshProUGUI>();
            _imgProgress = transform.GetComponentInChildren<Image>();
        }

        public void Populate(NetworkCharacter shipGs, PlayerConnectionInfo info)
        {
            string status = "";
            int progress = -1;
            switch (info.state)
            {
                case PlayerConnectState.Connecting:
                    status = (info.type == GGPOPlayerType.GGPO_PLAYERTYPE_LOCAL) ? "Local Player" : "Connecting...";
                    break;

                case PlayerConnectState.Synchronizing:
                    progress = info.connect_progress;
                    status = (info.type == GGPOPlayerType.GGPO_PLAYERTYPE_LOCAL) ? "Local Player" : "Synchronizing...";
                    break;

                case PlayerConnectState.Disconnected:
                    status = "Disconnected";
                    break;

                case PlayerConnectState.Disconnecting:
                    status = "Waiting for player...";
                    progress = (Utils.TimeGetTime() - info.disconnect_start) * 100 / info.disconnect_timeout;
                    break;
            }

            if (progress > 0)
            {
                _imgProgress.gameObject.SetActive(true);
                _imgProgress.fillAmount = progress / 100f;

                if (status.Length > 0)
                {
                    _txtStatus.gameObject.SetActive(true);
                    _txtStatus.text = status;
                }
                else
                {
                    _txtStatus.gameObject.SetActive(false);
                }
            }
            else
            {
                _imgProgress.gameObject.SetActive(false);
                _txtStatus.gameObject.SetActive(false);
            }
        }
    }
}