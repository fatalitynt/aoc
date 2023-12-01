def convert_letter(char):
    if ord(char) < ord("a"):
        return ord(char) - ord("A") + 27
    return ord(char) - ord("a") + 1


def handle_line(line):  # part 1
    n = len(line)
    left = line[:int(n/2)]
    right = line[int(n/2):]
    for c1 in left:
        for c2 in right:
            if c1 == c2:
                return convert_letter(c1)
    return -1


def handle_group(lines):  # part 2
    for c1 in lines[0]:
        for c2 in lines[1]:
            if c1 == c2:
                for c3 in lines[2]:
                    if c1 == c3:
                        return convert_letter(c1)
    return 0

def main():
    filename = "_input.txt"

    with open(filename) as file:
        sum = 0
        lines = list(map(lambda x: x.rstrip(), file))
        n = int(len(lines)/3)
        for i in range(n):
            sum += handle_group(lines[i*3: i*3+3])
        print(sum)


if __name__ == '__main__':
    main()

'''
What I learned?
- get int from char by ord()
'''
