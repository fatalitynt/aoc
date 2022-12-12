from collections import deque

dx = [1, -1, 0, 0]
dy = [0, 0, -1, 1]


def get_neighs(p, h, w, dist, dp):
    res = []
    for i in range(4):
        nxt = (p[0] + dx[i], p[1] + dy[i])
        if 0 <= nxt[0] < h and 0 <= nxt[1] < w and nxt not in dist and dp[p] - dp[nxt] < 2:
            res.append(nxt)
    return res


def find(e, h, w, dp):
    q = deque()
    q.append(e)
    dist = {e: 0}
    while len(q) > 0:
        p = q.popleft()
        p_dist = dist[p]
        for ng in get_neighs(p, h, w, dist, dp):
            dist[ng] = p_dist + 1
            q.append(ng)
    return dist


def main():
    filename = "_input.txt"
    with open(filename) as file:
        lines = list(map(lambda y: y.rstrip(), file))
    dp = {}
    s = (0, 0)
    e = (0, 0)
    h = len(lines)
    w = len(lines[0])
    starts = []
    for i in range(h):
        for j in range(w):
            p = (i, j)
            if lines[i][j] == 'S':
                s = p
                dp[p] = 0
                starts.append(p)
            elif lines[i][j] == 'E':
                e = (i, j)
                dp[p] = ord('z') - ord('a')
            else:
                dp[p] = ord(lines[i][j]) - ord('a')
                if lines[i][j] == 'a':
                    starts.append(p)
    dist = find(e, h, w, dp)
    print(dist[s])
    print(min(map(lambda x: dist[x], filter(lambda x: x in dist, starts))))


if __name__ == '__main__':
    main()

'''
What I learned?
- from collections import deque
'''