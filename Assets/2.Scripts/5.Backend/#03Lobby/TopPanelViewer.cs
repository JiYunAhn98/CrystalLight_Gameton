using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TopPanelViewer : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _textRevive;
	[SerializeField] TextMeshProUGUI _textGold;
	[SerializeField] TextMeshProUGUI _accessoryCoupon;
	[SerializeField] TextMeshProUGUI _faceCoupon;

	private void Awake()
	{
		BackendGameData.Instance.onGameDataLoadEvent.AddListener(UpdateGameData);
		BackendGameData.Instance.onGameDataLoadEvent.AddListener(UpdateGameGoldData);
		BackendGameData.Instance.onGameDataLoadEvent.AddListener(UpdateGameReviveData);

		if(_accessoryCoupon != null) 
			BackendGameData.Instance.onGameDataLoadEvent.AddListener(UpdateGameAccessoryCouponData);
		if(_faceCoupon != null) 
			BackendGameData.Instance.onGameDataLoadEvent.AddListener(UpdateGameFaceCouponData);
	}
	public void UpdateGameData()
	{
		UpdateGameGoldData();
		UpdateGameReviveData();
		if (_accessoryCoupon != null) UpdateGameAccessoryCouponData();
		if (_faceCoupon != null) UpdateGameFaceCouponData();
	}
	public void UpdateGameGoldData()
	{
		if (BackendGameData.Instance.UserGameData.gold > 9999)
			_textGold.text = "9999+";
		else
			_textGold.text = $"{BackendGameData.Instance.UserGameData.gold}";
	}
	public void UpdateGameReviveData()
	{
		if (BackendGameData.Instance.UserGameData.heart > 99)
			_textRevive.text = "99+";
		else
			_textRevive.text = $"{BackendGameData.Instance.UserGameData.heart}";
	}
	public void UpdateGameAccessoryCouponData()
	{
		if (BackendGameData.Instance.UserGameData.accessoryTicket > 99)
			_accessoryCoupon.text = "99+";
		else
			_accessoryCoupon.text = $"{BackendGameData.Instance.UserGameData.accessoryTicket}";
	}
	public void UpdateGameFaceCouponData()
	{
		if (BackendGameData.Instance.UserGameData.faceTicket > 99)
			_faceCoupon.text = "99+";
		else
			_faceCoupon.text = $"{BackendGameData.Instance.UserGameData.faceTicket}";
	}
}

