public abstract class Packet<T>
{
    protected T m_data;
    protected int packetId;
    public void SetPacketId(int newId) { packetId = newId; }
    public int GetPacketId() { return packetId; }
    public T GetData() { return m_data; }
    public abstract byte[] GetPacketData(); // 바이너리 데이터 얻기
}