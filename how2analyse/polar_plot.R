# Lib
library(ggplot2)

# Seed
set.seed(20220315)

# Create random data from 0 to 360
example_dat <- data.frame(rot_y = runif(1000, min = 0, max = 360))


# Create polar plot
ggplot(example_dat, aes(x = rot_y )) +
  geom_histogram(binwidth = 15, boundary = -7.5, colour = "black", size = .25) +
  scale_x_continuous(limits = c(0,360),
                     breaks = seq(0, 360, by = 60),
                     minor_breaks = seq(0, 360, by = 15))  +
  coord_polar() +
  labs(title = 'Polar histogram of y rotation',
       y = 'Count',
       x = '')