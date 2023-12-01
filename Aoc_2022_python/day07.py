def mk_dir(name, parent):
    return {'name': name, 'parent': parent, 'folders': [], 'files': []}


root = mk_dir('/', {})
cur = {}
res = 0
avail_space = 70000000
req_space = 30000000
dir_sizes = []


def try_parse_size(s):
    try:
        return int(s)
    except ValueError:
        return -1


def handle_output(s):
    parts = s.split()
    size = try_parse_size(parts[0])
    if size < 0:
        return
    file = {'name': parts[1], 'size': size}
    cur['files'].append(file)


def handle_cd(s):
    global cur
    if s == '..':
        cur = cur['parent']
    elif s == '/':
        cur = root
    else:
        next_dir = mk_dir(s, cur)
        cur['folders'].append(next_dir)
        cur = next_dir


def handle_cmd(s):
    if s.startswith("cd"):
        handle_cd(s[3:])


def process_line(s):
    if s.startswith("$"):
        handle_cmd(s[2:])
    else:
        handle_output(s)


def recalc_total_size(f):
    global res
    global dir_sizes
    for nested in f['folders']:
        recalc_total_size(nested)
    files_size = sum(map(lambda x: x['size'], f['files']))
    folders_size = sum(map(lambda x: x['size'], f['folders']))
    size = files_size + folders_size
    f['size'] = size
    dir_sizes.append(size)
    if size <= 100000:
        res += size


def main():
    filename = "_input.txt"

    with open(filename) as file:
        lines = list(map(lambda x: x.rstrip(), file))

        for line in lines:
            process_line(line)

        recalc_total_size(root)
        free_space = avail_space - root['size']
        # part 1
        # print(res)
        # part 2
        print(min(filter(lambda x: free_space + x >= req_space, dir_sizes)))


if __name__ == '__main__':
    main()

'''
What I learned?
- nothing
'''
