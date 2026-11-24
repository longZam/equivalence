using System;
using System.Runtime.CompilerServices;
using System.Text;

namespace equivalence;

/// <summary>
/// int 정수로 이루어진 5x5 행렬 구조체 (값 형식, .NET 8 이상).
/// </summary>
public struct Matrix5x5 : IEquatable<Matrix5x5>
{
    public const int Size = 5;

    // 25개의 int를 구조체 내부에 인라인으로 저장 (값 복사 시 완전히 복제됨)
    [InlineArray(Size * Size)]
    private struct Buffer
    {
        private int _element0;
    }

    private Buffer _data;

    /// <summary>[row, col] 로 요소에 접근합니다. 범위: 0~4</summary>
    public int this[int row, int col]
    {
        get
        {
            CheckRange(row, col);
            return _data[row * Size + col];
        }
        set
        {
            CheckRange(row, col);
            _data[row * Size + col] = value;
        }
    }

    public static Matrix5x5 Transpose(in Matrix5x5 matrix)
    {
        Matrix5x5 result = default;
        for (int i = 0; i < Size; i++)
            for (int j = 0; j < Size; j++)
                result[j, i] = matrix[i, j];
        return result;
    }

    /// <summary>단위행렬을 반환합니다.</summary>
    public static Matrix5x5 Identity
    {
        get
        {
            Matrix5x5 m = default;
            for (int i = 0; i < Size; i++)
                m[i, i] = 1;
            return m;
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void CheckRange(int row, int col)
    {
        if ((uint)row >= Size)
            throw new IndexOutOfRangeException($"row는 0~{Size - 1} 범위여야 합니다. (값: {row})");
        if ((uint)col >= Size)
            throw new IndexOutOfRangeException($"col은 0~{Size - 1} 범위여야 합니다. (값: {col})");
    }

    // ---- 동등성 비교 ----
    public readonly bool Equals(Matrix5x5 other)
    {
        for (int i = 0; i < Size * Size; i++)
            if (_data[i] != other._data[i]) return false;
        return true;
    }

    public override readonly bool Equals(object? obj) => obj is Matrix5x5 m && Equals(m);

    public override readonly int GetHashCode()
    {
        var hc = new HashCode();
        for (int i = 0; i < Size * Size; i++) hc.Add(_data[i]);
        return hc.ToHashCode();
    }

    /// <summary>행렬곱 (a x b). 결과의 [i, j] = Σk a[i, k] * b[k, j]</summary>
    public static Matrix5x5 operator *(Matrix5x5 a, Matrix5x5 b)
    {
        Matrix5x5 result = default;
        for (int i = 0; i < Size; i++)
        {
            for (int j = 0; j < Size; j++)
            {
                int sum = 0;
                for (int k = 0; k < Size; k++)
                    sum += a._data[i * Size + k] * b._data[k * Size + j];
                result._data[i * Size + j] = sum;
            }
        }
        return result;
    }

    public static bool operator ==(Matrix5x5 a, Matrix5x5 b) => a.Equals(b);
    public static bool operator !=(Matrix5x5 a, Matrix5x5 b) => !a.Equals(b);

    public override readonly string ToString()
    {
        var sb = new StringBuilder();
        for (int i = 0; i < Size; i++)
        {
            for (int j = 0; j < Size; j++)
            {
                sb.Append(_data[i * Size + j].ToString().PadLeft(4));
            }
            sb.AppendLine();
        }
        return sb.ToString();
    }
}