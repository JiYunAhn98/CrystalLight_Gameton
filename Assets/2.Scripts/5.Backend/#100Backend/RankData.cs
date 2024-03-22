using UnityEngine;
using TMPro;

public class RankData : MonoBehaviour
{
    //��ŷ�� ���� �����͵� ����

    [SerializeField] TextMeshProUGUI textRank;
    [SerializeField] TextMeshProUGUI textNickname;
    [SerializeField] TextMeshProUGUI textScore;

    private string _rank;
    private string _nickname;
    public string _score;

    public string Rank
    {
        set
        {
            _rank = value;
            textRank.text = _rank;
        }
        get => _rank;
    }

    public string Nickname
    {
        set
        {
            _nickname = value;
            textNickname.text = _nickname;
        }
        get => _nickname;
    }

    public string Score
    {
        set
        {
            _score = value;
            textScore.text = _score;
        }
        get => _score;
    }
}