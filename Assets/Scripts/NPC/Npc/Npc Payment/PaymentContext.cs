[System.Serializable]
public class PaymentContext
{
    public float totalPrice;
    public PaymentType method;          // Cash or Card
    public NpcController payer;         // 누가 계산하나

    // 금액 계산 로직을 나중에 여기에 추가 (세금·할인 등)
    public void Calculate() { /* TODO */ }
}

public enum PaymentType { Cash, Card }