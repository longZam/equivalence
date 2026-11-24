using equivalence;

static bool IsReflexive(in Matrix5x5 matrix)
{
    for (int i = 0; i < 5; i++)
    {
        if (matrix[i, i] == 0)
            return false;
    }

    return true;
}

static bool IsSymmetric(in Matrix5x5 matrix)
{
    for (int i = 0; i < 5; i++)
    {
        for (int j = 0; j < 5; j++)
        {
            if (matrix[i, j] != matrix[j, i])
                return false;
        }
    }

    return true;
}

static bool IsTransitive(in Matrix5x5 matrix)
{
    var r2 = matrix * matrix;

    for (int i = 0; i < 5; i++)
    {
        for (int j = 0; j < 5; j++)
        {
            if (r2[i, j] == 1 && matrix[i, j] == 0)
                return false;
        }
    }

    return true;
}

static bool IsEquivalence(in Matrix5x5 matrix)
{
    return IsReflexive(in matrix) && IsSymmetric(in matrix) && IsTransitive(in matrix);
}

static void PrintMatrix5x5(in Matrix5x5 matrix)
{
    for (int i = 0; i < 5; i++)
    {
        Console.Write('[');

        for (int j = 0; j < 5; j++)
        {
            Console.Write(matrix[i, j]);

            if (j != 4)
                Console.Write(", ");
        }

        Console.WriteLine(']');
    }
}

static void PrintEquivalenceClass(in Matrix5x5 equivalence)
{
    Span<bool> visited = stackalloc bool[5];
    visited.Clear();

    for (int i = 0; i < 5; i++)
    {
        if (visited[i])
            continue;

        Console.Write("[");

        for (int j = 0; j < 5; j++)
        {
            if (equivalence[i, j] == 1)
            {
                visited[j] = true;
                Console.Write($"{j + 1} ");
            }
        }

        Console.WriteLine("]");
    }
}


static Matrix5x5 GetReflexiveClosure(in Matrix5x5 matrix)
{
    Matrix5x5 output = matrix;

    for (int i = 0; i < 5; i++)
    {
        output[i, i] = 1;
    }

    return output;
}

static Matrix5x5 GetSymmetricClosure(in Matrix5x5 matrix)
{
    var result = matrix;
    var transpose = Matrix5x5.Transpose(in matrix);
    
    for (int i = 0; i < 5; i++)
    {
        for (int j = 0; j < 5; j++)
        {
            result[i, j] |= transpose[i, j];
        }
    }

    return result;
}

static Matrix5x5 GetTransitiveClosure(in Matrix5x5 matrix)
{
    Matrix5x5 result = matrix;
    
    for (int k = 0; k < 5; k++)
    {
        for (int i = 0; i < 5; i++)
        {
            for (int j = 0; j < 5; j++)
            {
                if (result[i, j] == 0 && result[i, k] == 1 && result[k, j] == 1)
                    result[i, j] = 1;
            }
        }
    }

    return result;
}



var matrix = new Matrix5x5();

for (int i = 0; i < 5; i++)
{
    string input = Console.ReadLine() ?? throw new InvalidDataException();
    var row = input.Split();

    for (int j = 0; j < 5; j++)
        matrix[i, j] = int.Parse(row[j]);
}

if (IsReflexive(in matrix))
    Console.WriteLine("이 관계행렬은 반사 관계입니다.");
else
{
    Console.WriteLine("이 관계행렬은 반사 관계가 아닙니다.");
    Console.WriteLine("이 관계행렬의 반사 폐포 변환 전:");
    PrintMatrix5x5(in matrix);
    Console.WriteLine("이 관계행렬의 반사 폐포 변환 후:");
    Matrix5x5 closure = GetReflexiveClosure(in matrix);
    PrintMatrix5x5(closure);

    if (IsEquivalence(in closure))
    {
        Console.WriteLine("이 폐포는 동치 관계입니다.");
        Console.WriteLine("동치류:");
        PrintEquivalenceClass(in closure);
    }
    else
    {
        Console.WriteLine("이 폐포는 동치 관계가 아닙니다.");
    }
}

if (IsSymmetric(in matrix))
    Console.WriteLine("이 관계행렬은 대칭 관계입니다.");
else
{
    Console.WriteLine("이 관계행렬은 대칭 관계가 아닙니다.");
    Console.WriteLine("이 관계행렬의 대칭 폐포 변환 전:");
    PrintMatrix5x5(in matrix);
    Console.WriteLine("이 관계행렬의 대칭 폐포 변환 후:");
    Matrix5x5 closure = GetSymmetricClosure(in matrix);
    PrintMatrix5x5(closure);

    if (IsEquivalence(in closure))
    {
        Console.WriteLine("이 폐포는 동치 관계입니다.");
        Console.WriteLine("동치류:");
        PrintEquivalenceClass(in closure);
    }
    else
    {
        Console.WriteLine("이 폐포는 동치 관계가 아닙니다.");
    }
}

if (IsTransitive(in matrix))
    Console.WriteLine("이 관계행렬은 추이 관계입니다.");
else
{
    Console.WriteLine("이 관계행렬은 추이 관계가 아닙니다.");
    Console.WriteLine("이 관계행렬의 추이 폐포 변환 전:");
    PrintMatrix5x5(in matrix);
    Console.WriteLine("이 관계행렬의 추이 폐포 변환 후:");
    Matrix5x5 closure = GetTransitiveClosure(in matrix);
    PrintMatrix5x5(closure);

    if (IsEquivalence(in closure))
    {
        Console.WriteLine("이 폐포는 동치 관계입니다.");
        Console.WriteLine("동치류:");
        PrintEquivalenceClass(in closure);
    }
    else
    {
        Console.WriteLine("이 폐포는 동치 관계가 아닙니다.");
    }
}