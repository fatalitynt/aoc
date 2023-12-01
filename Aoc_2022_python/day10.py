def main():
    filename = "_input.txt"
    with open(filename) as file:
        lines = list(map(lambda y: y.rstrip(), file))

        pref = [1]
        for line in lines:
            p = line.split()
            pref.append(pref[-1])
            if len(p) > 1:
                val = int(p[1])
                pref.append(pref[-1] + val)

        res = 0
        for i in range(len(pref)):
            if i % 40 == 20:
                res += i * pref[i-1]
        # part 1
        # print(res)

        text = []
        for i in range(240):
            pos = i % 40
            crt = pref[i]
            text.append('#' if abs(pos - crt) < 2 else '.')

        for i in range(6):
            print("".join(text[40 * i:40*(i+1)]))


if __name__ == '__main__':
    main()

'''
What I learned?
- nothing
'''
