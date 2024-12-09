namespace AoCDay8Puzzle1;

abstract record InputField()
{
    public static InputField Parse(char sign)
    {
        return sign switch
        {
            '.' => new EmptyField(),
            _ => new AntennaField(sign),
        };
    }
}