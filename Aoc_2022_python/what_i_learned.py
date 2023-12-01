def main():
    res = []
    for i in range(1, 26):
        filename = "day" + ("0" if i < 10 else "") + str(i) + ".py"
        with open(filename) as file:
            lines = [line.rstrip() for line in file]
        for knowledge in lines[lines.index("What I learned?") + 1:-1]:
            if not knowledge.endswith("nothing"):
                res.append(knowledge)

    max_len = max(map(lambda r: len(r), res))
    title = " What I learned in Python "
    dash_count = (max_len - len(title)) // 2
    print("+-" + "-" * dash_count + title + "-" * dash_count + "-+")
    for i in res:
        print("| " + i + " " * (max_len - len(i)) + " |")
    print("+-" + "-" * max_len + "-+")


if __name__ == '__main__':
    main()
