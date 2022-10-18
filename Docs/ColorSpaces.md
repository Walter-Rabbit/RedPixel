# **Цветовые пространства**

## **RGB**

R - Red (красный) [0; 255]

G - Green (зелёный) [0; 255]

B - Blue (синий) [0; 255]

## **HSL**

H - Hue (тон) [0; 360]

S - Saturation (насыщенность) [0; 1]

L - Lightness (светлота) [0; 1]

### **Конвертация из RGB в HSL**

$i_k = \frac{i}{255}, \quad i = R, G, B$

$MAX = max(R_k, G_k, B_k)$

$MIN = min(R_k, G_k, B_k)$

$$H = \begin{cases}
undefined, \quad MAX = MIN \\
60^o \cdot \frac{G_k - B_k}{MAX - MIN} + 0^o, \quad MAX = R_k \quad and \quad  G_k \ge B_k \\
60^o \cdot \frac{G_k - B_k}{MAX - MIN} + 360^o, \quad MAX = R_k \quad and \quad G_k < B_k \\
60^o \cdot \frac{B_k - R_k}{MAX - MIN} + 120^o, \quad MAX = G_k \\
60^o \cdot \frac{R_k - G_k}{MAX - MIN} + 240^o,\quad MAX = B_k
\end{cases}$$

$$S = \begin{cases}
0, \quad MAX = -MIN \\
\frac{MAX - MIN}{1 - |1 - (MAX + MIN)|}, \quad MAX \neq -MIN
\end{cases}$$

$L = \frac{1}{2}(MAX + MIN)$

### **Конвертация в RGB из HSL**

$$Q = \begin{cases}
L \cdot (1 + S), \quad L < 0.5\\
L + S - (L \cdot S), \quad L \ge 0.5
\end{cases}$$

$H_k = \frac{H}{360}$

$T_R = H_k + \frac{1}{3}$

$T_G = H_k$

$T_B = H_k - \frac{1}{3}$

$$T_i = \begin{cases}
T_i + 1, \quad T_i < 0 \\
T_i, \quad 0 \le T_i \le 1 \\
T_i - 1, \quad T_i > 1
\end{cases}, \quad i = R, G, B$$

$$i_k = \begin{cases}
P + ((Q - P) \cdot 6 \cdot T_i), \quad T_i < \frac{1}{6} \\
Q, \quad \frac{1}{6} \le T_i < \frac{1}{2} \\
P + ((Q - P) \cdot (\frac{2}{3} - T_i) \cdot 6), \quad \frac{1}{2} \le T_i < \frac{2}{3} \\
P, \quad T_i \ge \frac{2}{3}
\end{cases}, \quad i = R, G, B$$

$i = i_k \cdot 255, \quad i = R, G, B$

## **HSV**

H - Hue (тон) [0; 360]

S - Saturation (насыщенность) [0; 1]

V - Value (значение) [0; 1]

### **Конвертация из RGB в HSV**

$i_k = \frac{i}{255}, \quad i = R, G, B$

$MAX = max(R_k, G_k, B_k)$

$MIN = min(R_k, G_k, B_k)$

$$H = \begin{cases}
undefined, \quad MAX = MIN \\
60^o \cdot \frac{G_k - B_k}{MAX - MIN} + 0^o, \quad MAX = R_k \quad and \quad  G_k \ge B_k \\
60^o \cdot \frac{G_k - B_k}{MAX - MIN} + 360^o, \quad MAX = R_k \quad and \quad G_k < B_k \\
60^o \cdot \frac{B_k - R_k}{MAX - MIN} + 120^o, \quad MAX = G_k \\
60^o \cdot \frac{R_k - G_k}{MAX - MIN} + 240^o,\quad MAX = B_k
\end{cases}$$

$$S = \begin{cases}
0, \quad MAX = 0 \\
1 - \frac{MIN}{MAX}, \quad MAX \neq 0
\end{cases}$$

$V = MAX$

### **Конвертация в RGB из HSV**

$S_k = S \cdot 100$

$V_k = V \cdot 100$

$H_i = \frac{H}{60} mod(6)$

$V_{min} = \frac{(100 - S) \cdot V}{100}$

$a = (V - V_{min}) \cdot \frac{H mod(60)}{60}$

$V_{inc} = V_{min} + a$

$V_{dec} = V - a$

| $H_i$ | $R_k$ | $G_k$ | $B_k$ |
| --- | --- | --- | --- |
| $0$ | $V$ | $V_{inc}$ | $V_{min}$ |
| $1$ | $V_{dec}$ | $V$ | $V_{min}$ |
| $2$ | $V_{min}$ | $V$ | $V_{inc}$ |
| $3$ | $V_{min}$ | $V_{dec}$ | $V$ |
| $4$ | $V_{inc}$ | $V_{min}$ | $V$ |
| $5$ | $V$ | $V_{min}$ | $V_{dec}$ |

$i = i_k \cdot \frac{255}{100}, \quad i = R, G, B$

## **YCbCr.601**

Y - Яркость

Cb - Синяя цветоразностная компонента

Cr - Красная цветоразностная компонента

601 - Формат для коддирования видеосигнала

### **Конвертация из RGB в YCbCr.601**

$Y = 16 + \frac{65.738 \cdot R}{256} + \frac{129.057 \cdot G}{256} + \frac{25.064 \cdot B}{256}$

$C_B = 128 + \frac{-37.945 \cdot R}{256} - \frac{74.494 \cdot G}{256} + \frac{112.439 \cdot B}{256}$

$C_R = 128 + \frac{112.439 \cdot R}{256} - \frac{94.154 \cdot G}{256} - \frac{18.285 \cdot B}{256}$

### **Конвертация в RGB из YCbCr.601**

$R = \frac{298.082 \cdot Y}{256} + \frac{408.583 \cdot C_R}{256} - 222.921$

$G = \frac{296.082 \cdot Y}{256} - \frac{100.291 \cdot C_B}{256} - \frac{208.120 \cdot C_R}{256} + 135.576$

$B = \frac{298.082 \cdot Y}{256} + \frac{516.412 \cdot C_B}{256} - 276.836$

## **YCbCr.709**

Y - Яркость

Cb - Синяя цветоразностная компонента

Cr - Красная цветоразностная компонента

709 - Формат для коддирования видеосигнала

### **Конвертация из RGB в YCbCr.709**

$Y = 0.299 \cdot R + 0.587 \cdot G + 0.114 \cdot B$

$C_B = 128 - 0.168736 \cdot R - 0.331264 \cdot G + 0.5 \cdot B$

$C_R = 128 + 0.5 \cdot R - 0.418688 \cdot G - 0.081312 \cdot B$

### **Конвертация в RGB из YCbCr.709**

$R = Y + 1.402 \cdot (C_R - 128)$

$G = Y - 0.34414 \cdot (C_B - 128) - 0.71414 \cdot (C_R - 128)$

$B = Y + 1.772 \cdot (C_B - 128)$

## **CMY**

C - Cyan (голубой) [0; 1]

M - Magenta (пурпурный) [0; 1]

Y - Yellow (жёлтый) [0; 1]

### **Конвертация из RGB в CMY**

$C = 1 - \frac{R}{255}$

$M = 1 - \frac{G}{255}$

$Y = 1 - \frac{B}{255}$

### **Конвертация в RGB из CMY**

$R = 255 \cdot (1 - C)$

$G = 255 \cdot (1 - M)$

$B = 255 \cdot (1 - Y)$
