def check_part_overlap(al, ar, bl, br):
    return not(ar < bl or br < al)


def check_full_overlap(al, ar, bl, br):
    a_in_b = al >= bl and ar <= br
    b_in_a = bl >= al and br <= ar
    return a_in_b or b_in_a


def parse_and_check(s):
    pair = s.split(",")
    a = pair[0].split("-")
    b = pair[1].split("-")
    al = int(a[0])
    ar = int(a[1])
    bl = int(b[0])
    br = int(b[1])
    return check_part_overlap(al, ar, bl, br)


def main():
    filename = "_input.txt"

    with open(filename) as file:
        lines = list(map(lambda x: x.rstrip(), file))
        res = len(list(filter(parse_and_check, lines)))
        print(res)


if __name__ == '__main__':
    main()

'''
What I learned?
- nothing new
'''
