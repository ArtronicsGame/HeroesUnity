using System;
using System.Collections.Generic;
using System.Linq;

public class UDPPacket
{
    public char type;
    public List<int> iList;
    public List<float> fList;
    public string str = "";
    
    const int ISIZE = sizeof(int);
    const int FSIZE = sizeof(float);

    public UDPPacket()
    {
        iList = new List<int>();
        fList = new List<float>();
    }
    
    public UDPPacket(byte[] data)
    {
        int[] ia;
        float[] fa;
        (type, ia, fa, str) = Decode(data);
        iList = ia.ToList();
        fList = fa.ToList();
    }

    public byte[] Encode()
    {
        return Encode(type, iList.ToArray(), fList.ToArray(), str);
    }
    
    private byte[] Encode(char type, int[] ia, float[] fa, string s)
    {
        List<byte> data = new List<byte>();
        data.Add((byte) type);
        data.Add((byte) ia.Length);
        data.Add((byte) fa.Length);

        for (int i = 0; i < ia.Length; i++)
        {
            byte[] b = BitConverter.GetBytes(ia[i]);
            for (int j = 0; j < b.Length; j++)
                data.Add(b[j]);
        }
            
        for (int i = 0; i < fa.Length; i++)
        {
            byte[] b = BitConverter.GetBytes(fa[i]);
            for (int j = 0; j < b.Length; j++)
                data.Add(b[j]);
        }

        for (int i = 0; i < s.Length; i++)
        {
            data.Add(Convert.ToByte(s.ElementAt(i)));
        }

        return data.ToArray();
    }

    private (char type, int[] ia, float[] fa, string s) Decode(byte[] data)
    {
        char type = Convert.ToChar(data[0]);
        int iaSize = Convert.ToChar(data[1]);
        int faSize = Convert.ToChar(data[2]);

        int iaIndex = 3;
        int faIndex = iaIndex + iaSize * ISIZE;
        int sIndex = faIndex + faSize * FSIZE ;

        int[] ia = new int[iaSize];
        float[] fa = new float[faSize];
        string s = "";

        for (int i = 0, j = iaIndex; i < iaSize; i++, j += ISIZE)
            ia[i] = BitConverter.ToInt32(data, j);
            
        for (int i = 0, j = faIndex; i < faSize; i++, j += FSIZE)
            fa[i] = BitConverter.ToSingle(data, j);

        for (int i = sIndex; i < data.Length; i++)
            s += Convert.ToChar(data[i]);

        return (type, ia, fa, s);
    }
    
}