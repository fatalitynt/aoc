def main():
    filename = "_input.txt"
    with open(filename) as file:
        lines = [line.rstrip() for line in file]
    sums = []
    last_sum = 0
    for line in lines:
        if line == "":
            sums.append(last_sum)
            last_sum = 0
        else:
            last_sum += int(line)
    sums.append(last_sum)

    print("Top 1:", max(sums))
    sums.sort(reverse=True)
    print("Top 3 sum:", sum(sums[:3]))


if __name__ == '__main__':
    main()

'''
What I learned?
- how to open file and read all lines
- a.sort(reverse=True/False) works inplace
'''
